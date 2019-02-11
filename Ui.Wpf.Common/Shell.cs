using Autofac;
using MahApps.Metro.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using MahApps.Metro.SimpleChildWindow;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using IContainer = Autofac.IContainer;

namespace Ui.Wpf.Common
{
    public partial class Shell : ReactiveObject, IShell
    {
        public IContainer Container { get; set; }

        [Reactive] public string Title { get; set; }

        [Reactive] public IView SelectedView { get; set; }

        public void ShowView<TView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView
        {
            var layoutDocument = FindLayoutByViewRequest(DocumentPane, viewRequest);

            if (layoutDocument == null)
            {
                var view = Container.Resolve<TView>();
                if (options != null)
                    view.Configure(options);

                layoutDocument = new LayoutDocument
                {
                    ContentId = viewRequest?.ViewId,
                    Content = view
                };
                if (options != null)
                    layoutDocument.CanClose = options.CanClose;

                AddTitleRefreshing(view, layoutDocument);
                AddWindowBehaviour(view, layoutDocument);
                AddClosingByRequest(view, layoutDocument);

                DocumentPane.Children.Add(layoutDocument);

                InitializeView(view, viewRequest);
            }

            ActivateContent(layoutDocument, viewRequest);
        }

        public void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView
        {
            var layoutAnchorable = FindLayoutByViewRequest(ToolsPane, viewRequest);

            if (layoutAnchorable == null)
            {
                var view = Container.Resolve<TToolView>();
                if (options != null)
                    view.Configure(options);

                layoutAnchorable = new LayoutAnchorable
                {
                    ContentId = viewRequest?.ViewId,
                    Content = view,
                    CanAutoHide = false,
                    CanFloat = false,
                };
                view.ViewModel.CanClose = false;
                view.ViewModel.CanHide = false;

                AddTitleRefreshing(view, layoutAnchorable);
                AddWindowBehaviour(view, layoutAnchorable);

                ToolsPane.Children.Add(layoutAnchorable);

                InitializeView(view, viewRequest);
            }

            ActivateContent(layoutAnchorable, viewRequest);
        }

        public void ShowFlyoutView<TView>(
            ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
            where TView : class, IView
        {
            if (!string.IsNullOrEmpty(viewRequest?.ViewId))
            {
                var viewOld = FlyoutsControl.Items
                    .Cast<Flyout>()
                    .Select(x => (IView) x.Content)
                    .FirstOrDefault(x => (x.ViewModel as ViewModelBase)?.ViewId == viewRequest.ViewId);

                if (viewOld != null)
                {
                    (viewOld.ViewModel as IActivatableViewModel)?.Activate(viewRequest);
                    return;
                }
            }

            var view = Container.Resolve<TView>();
            if (options != null)
                view.Configure(options);

            options = options ?? new UiShowFlyoutOptions();

            var flyout = new Flyout
            {
                IsModal = options.IsModal,
                Position = options.Position,
                Theme = options.Theme,
                ExternalCloseButton = options.ExternalCloseButton,
                IsPinned = options.IsPinned,
                CloseButtonIsCancel = options.CloseButtonIsCancel,
                CloseCommand = options.CloseCommand,
                CloseCommandParameter = options.CloseCommandParameter,
                AnimateOpacity = options.AnimateOpacity,
                AreAnimationsEnabled = options.AreAnimationsEnabled,
                IsAutoCloseEnabled = options.IsAutoCloseEnabled,
                AutoCloseInterval = options.AutoCloseInterval,
                Width = options.Width ?? double.NaN,
                Height = options.Height ?? double.NaN,
                Content = view,
                IsOpen = true
            };

            var vm = view.ViewModel as ViewModelBase;
            if (vm != null)
            {
                Observable
                    .FromEventPattern<ViewModelCloseQueryArgs>(
                        x => vm.CloseQuery += x,
                        x => vm.CloseQuery -= x)
                    .Subscribe(x => flyout.IsOpen = false)
                    .DisposeWith(vm.Disposables);
            }

            var disposables = new CompositeDisposable();
            view.ViewModel
                .WhenAnyValue(x => x.Title)
                .Subscribe(x =>
                {
                    flyout.Header = x;
                    flyout.TitleVisibility =
                        !string.IsNullOrEmpty(x)
                            ? Visibility.Visible
                            : Visibility.Collapsed;
                })
                .DisposeWith(disposables);
            view.ViewModel
                .WhenAnyValue(x => x.CanClose)
                .Subscribe(x => flyout.CloseButtonVisibility = x ? Visibility.Visible : Visibility.Collapsed)
                .DisposeWith(disposables);

            Observable
                .FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    x => flyout.ClosingFinished += x,
                    x => flyout.ClosingFinished -= x)
                .Select(x => (Flyout) x.Sender)
                .Take(1)
                .Subscribe(x =>
                {
                    disposables.Dispose();
                    vm?.Closed(new ViewModelCloseQueryArgs());
                    x.Content = null;
                    FlyoutsControl.Items.Remove(x);
                });

            FlyoutsControl.Items.Add(flyout);

            InitializeView(view, viewRequest);

            (view.ViewModel as IActivatableViewModel)?.Activate(viewRequest);
        }

        public Task<TResult> ShowChildWindowView<TView, TResult>(
            ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
            where TView : class, IView
        {
            var view = Container.Resolve<TView>();
            if (options != null)
                view.Configure(options);

            InitializeView(view, viewRequest);
            (view.ViewModel as IActivatableViewModel)?.Activate(viewRequest);

            return Window.ShowChildWindowAsync<TResult>(
                new ChildWindowView(options) {Content = view},
                ChildWindowManager.OverlayFillBehavior.FullWindow
            );
        }

        public void ShowStartView<TStartWindow, TStartView>(
            UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView
        {
            if (options != null)
            {
                ToolPaneWidth = options.ToolPaneWidth;
                Title = options.Title;
            }

            var startObject = Container.Resolve<TStartWindow>() ??
                              throw new InvalidOperationException($"You should configure {typeof(TStartWindow)}");

            Window = startObject as Window ??
                     throw new InvalidCastException($"{startObject.GetType()} is not a window");

            ShowView<TStartView>();

            Window.Show();
        }

        public void ShowStartView<TStartWindow>(
            UiShowStartWindowOptions options = null)
            where TStartWindow : class
        {
            if (options != null)
            {
                ToolPaneWidth = options.ToolPaneWidth;
                Title = options.Title;
            }

            var startObject = Container.Resolve<TStartWindow>() ??
                              throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            Window = startObject as Window ??
                     throw new InvalidCastException($"{startObject.GetType()} is not a window");

            Window.Show();
        }

        public void AttachDockingManager(DockingManager dockingManager)
        {
            DockingManager = dockingManager;

            var layoutRoot = new LayoutRoot();
            DockingManager.Layout = layoutRoot;

            DocumentPane = layoutRoot.RootPanel.Children[0] as LayoutDocumentPane;

            ToolsPane = new LayoutAnchorablePane();
            layoutRoot.RootPanel.Children.Insert(0, ToolsPane);
            ToolsPane.DockWidth = new GridLength(ToolPaneWidth.GetValueOrDefault(410));
        }

        public void AttachFlyoutsControl(FlyoutsControl flyoutsControl)
        {
            FlyoutsControl = flyoutsControl;
        }

        private static T FindLayoutByViewRequest<T>(LayoutGroup<T> layoutGroup, ViewRequest viewRequest)
            where T : LayoutContent
        {
            return viewRequest?.ViewId != null
                ? layoutGroup.Children.FirstOrDefault(x => x.ContentId == viewRequest.ViewId)
                : null;
        }

        private static void InitializeView(IView view, ViewRequest viewRequest)
        {
            if (view.ViewModel is ViewModelBase vmb)
            {
                vmb.ViewId = viewRequest?.ViewId;
            }

            if (view.ViewModel is IInitializableViewModel initializibleViewModel)
            {
                initializibleViewModel.Initialize(viewRequest);
            }
        }

        private static void ActivateContent(LayoutContent layoutContent, ViewRequest viewRequest)
        {
            layoutContent.IsActive = true;
            if (layoutContent.Content is IView view &&
                view.ViewModel is IActivatableViewModel activatableViewModel)
            {
                activatableViewModel.Activate(viewRequest);
            }
        }

        private static void AddClosingByRequest<TView>(TView view, LayoutContent layoutDocument)
            where TView : class, IView
        {
            if (!(view.ViewModel is ViewModelBase baseViewModel))
                return;

            Observable
                .FromEventPattern<ViewModelCloseQueryArgs>(
                    x => baseViewModel.CloseQuery += x,
                    x => baseViewModel.CloseQuery -= x)
                .Subscribe(x => { layoutDocument.Close(); })
                .DisposeWith(baseViewModel.Disposables);

            Observable
                .FromEventPattern<CancelEventArgs>(
                    x => layoutDocument.Closing += x,
                    x => layoutDocument.Closing -= x)
                .Subscribe(x =>
                {
                    var vcq = new ViewModelCloseQueryArgs {IsCanceled = false};
                    baseViewModel.Closing(vcq);

                    if (vcq.IsCanceled)
                    {
                        x.EventArgs.Cancel = true;
                    }
                })
                .DisposeWith(baseViewModel.Disposables);

            Observable
                .FromEventPattern(
                    x => layoutDocument.Closed += x,
                    x => layoutDocument.Closed -= x)
                .Subscribe(_ => baseViewModel.Closed(new ViewModelCloseQueryArgs {IsCanceled = false}))
                .DisposeWith(baseViewModel.Disposables);
        }

        private static void AddTitleRefreshing<TView>(TView view, LayoutContent layoutDocument)
            where TView : class, IView
        {
            var titleRefreshSubsription = view.ViewModel
                .WhenAnyValue(vm => vm.Title)
                .Subscribe(x => layoutDocument.Title = x);

            Observable
                .FromEventPattern(
                    x => layoutDocument.Closed += x,
                    x => layoutDocument.Closed -= x)
                .Take(1)
                .Subscribe(_ => titleRefreshSubsription.Dispose());
        }

        private static void AddWindowBehaviour<TView>(TView view, LayoutContent layoutContent)
            where TView : class, IView
        {
            if (!(view.ViewModel is ViewModelBase baseViewModel))
                return;

            baseViewModel
                .WhenAnyValue(x => x.IsEnabled)
                .Subscribe(x => layoutContent.IsEnabled = x)
                .DisposeWith(baseViewModel.Disposables);

            baseViewModel
                .WhenAnyValue(x => x.CanClose)
                .Subscribe(x => layoutContent.CanClose = x)
                .DisposeWith(baseViewModel.Disposables);

            if (layoutContent is LayoutAnchorable layoutAnchorable)
            {
                baseViewModel
                    .WhenAnyValue(x => x.CanHide)
                    .Subscribe(x => layoutAnchorable.CanHide = x)
                    .DisposeWith(baseViewModel.Disposables);
            }
        }


        //TODO replace to abstract manager
        private DockingManager DockingManager { get; set; }
        private FlyoutsControl FlyoutsControl { get; set; }

        protected LayoutDocumentPane DocumentPane { get; set; }
        private LayoutAnchorablePane ToolsPane { get; set; }
        private Window Window { get; set; }

        private int? ToolPaneWidth { get; set; }
    }
}