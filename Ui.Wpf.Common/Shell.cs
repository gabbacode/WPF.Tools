using Autofac;
using MahApps.Metro.Controls;
using MahApps.Metro.SimpleChildWindow;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Ui.Wpf.Common.AttachedProperties;
using Ui.Wpf.Common.DockingManagers;
using Ui.Wpf.Common.Extensions;
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

        [Reactive] public DockingManager DockingManager { get; set; }

        public FlyoutsControl FlyoutsControl { get; set; }

        private Window Window { get; set; }

        public void CloseView(string viewId)
        {
            CloseViewIn(DefaultDockingManager.Views, viewId);
        }

        public void CloseTool(string viewId)
        {
            CloseToolIn(DefaultDockingManager.Tools, viewId);
        }

        public void CloseViewIn(string containerName, string viewId)
        {
            var container = DockingManager.FindObjectByName<LayoutGroup<LayoutContent>>(containerName);
            var layout = container.FindByViewId<LayoutContent>(viewId);
            CloseContent(layout);
        }

        public void CloseToolIn(string containerName, string viewId)
        {
            var container = DockingManager.FindObjectByName<LayoutGroup<LayoutAnchorable>>(containerName);
            var layout = container.FindByViewId<LayoutContent>(viewId);
            CloseContent(layout);
        }

        public void ShowView(
            Func<ILifetimeScope, IView> viewFactory,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
        {
            ShowViewIn(DefaultDockingManager.Views, viewFactory, viewRequest, options);
        }

        public void ShowViewIn(
            string containerName,
            Func<ILifetimeScope, IView> viewFactory,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
        {
            var documentPane = DockingManager.FindObjectByName<LayoutGroup<LayoutContent>>(containerName);
            var layoutDocument = documentPane.FindByViewRequest<LayoutContent>(viewRequest);

            if (layoutDocument == null)
            {
                var view = viewFactory(Container);
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

                documentPane.Children.Add(layoutDocument);

                InitializeView(view, viewRequest);
            }

            ActivateContent(layoutDocument, viewRequest);
        }

        public void ShowView<TView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView
        {
            ShowViewIn(DefaultDockingManager.Views,
                container => container.Resolve<TView>(),
                viewRequest,
                options);
        }

        public void ShowViewIn<TView>(
            string containerName,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView
        {
            ShowViewIn(containerName,
                container => container.Resolve<TView>(),
                viewRequest,
                options);
        }

        public void ShowTool(
            Func<ILifetimeScope, IToolView> toolFactory,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
        {
            ShowToolIn(DefaultDockingManager.Tools, toolFactory, viewRequest, options);
        }

        public void ShowToolIn(
            string containerName,
            Func<ILifetimeScope, IToolView> toolFactory,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
        {
            var toolsPane = DockingManager.FindObjectByName<LayoutGroup<LayoutAnchorable>>(containerName);
            var layoutAnchorable = toolsPane.FindByViewRequest<LayoutAnchorable>(viewRequest);

            if (layoutAnchorable == null)
            {
                var view = toolFactory(Container);
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
                AddClosingByRequest(view, layoutAnchorable);

                toolsPane.Children.Add(layoutAnchorable);

                InitializeView(view, viewRequest);
            }

            ActivateContent(layoutAnchorable, viewRequest);
        }

        public void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView
        {
            ShowToolIn(DefaultDockingManager.Tools,
                container => container.Resolve<TToolView>(),
                viewRequest,
                options);
        }

        public void ShowToolIn<TToolView>(
            string containerName,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView
        {
            ShowToolIn(containerName,
                container => container.Resolve<TToolView>(),
                viewRequest,
                options);
        }

        public async Task<TResult> ShowFlyoutView<TResult>(Func<ILifetimeScope, IView> viewFactory,
            ViewRequest viewRequest = null, UiShowFlyoutOptions options = null)
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
                    return default(TResult);
                }
            }

            var view = viewFactory(Container);
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
                CloseButtonIsCancel = options.CloseByEscape,
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

            var closedObservable = Observable
                .FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    x => flyout.ClosingFinished += x,
                    x => flyout.ClosingFinished -= x);

            FlyoutsControl.Items.Add(flyout);
            InitializeView(view, viewRequest);
            (view.ViewModel as IActivatableViewModel)?.Activate(viewRequest);

            await closedObservable.FirstAsync();

            disposables.Dispose();
            vm?.Closed(new ViewModelCloseQueryArgs());
            flyout.Content = null;
            FlyoutsControl.Items.Remove(flyout);

            if (vm is IResultableViewModel<TResult> model)
                return model.ViewResult;

            return default(TResult);
        }

        public Task<TResult> ShowFlyoutView<TView, TResult>(
            ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
            where TView : class, IView
        {
            return ShowFlyoutView<TResult>(
                container => container.Resolve<TView>(),
                viewRequest,
                options);
        }

        public async void ShowFlyoutView(Func<ILifetimeScope, IView> viewFactory, ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
        {
            await ShowFlyoutView<Unit>(viewFactory, viewRequest, options);
        }

        public async void ShowFlyoutView<TView>(
            ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
            where TView : class, IView
        {
            await ShowFlyoutView<TView, Unit>();
        }

        public Task<TResult> ShowChildWindowView<TResult>(Func<ILifetimeScope, IView> viewFactory,
            ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
        {
            if (!string.IsNullOrEmpty(viewRequest?.ViewId))
            {
                var metroDialogContainer = Window.Template.FindName("PART_MetroActiveDialogContainer", Window) as Grid;
                metroDialogContainer = metroDialogContainer ??
                                       Window.Template.FindName("PART_MetroInactiveDialogsContainer", Window) as Grid;

                if (metroDialogContainer == null)
                    throw new InvalidOperationException(
                        "The provided child window can not add, there is no container defined.");

                var viewOld = metroDialogContainer.Children
                    .Cast<ChildWindowView>()
                    .Select(x => (IView) x.Content)
                    .FirstOrDefault(x => (x.ViewModel as ViewModelBase)?.ViewId == viewRequest.ViewId);

                if (viewOld != null)
                {
                    (viewOld.ViewModel as IActivatableViewModel)?.Activate(viewRequest);
                    return Task.FromResult(default(TResult));
                }
            }

            var view = viewFactory(Container);
            if (options != null)
                view.Configure(options);

            InitializeView(view, viewRequest);
            (view.ViewModel as IActivatableViewModel)?.Activate(viewRequest);

            options = options ?? new UiShowChildWindowOptions();

            var childWindow = new ChildWindowView(options) {Content = view};

            var vm = view.ViewModel as ViewModelBase;
            if (vm != null)
            {
                Observable
                    .FromEventPattern<ViewModelCloseQueryArgs>(
                        x => vm.CloseQuery += x,
                        x => vm.CloseQuery -= x)
                    .Subscribe(x =>
                    {
                        if (vm is IResultableViewModel<TResult> model)
                            childWindow.Close(model.ViewResult);
                        else
                            childWindow.Close();
                    })
                    .DisposeWith(vm.Disposables);

                Observable
                    .FromEventPattern<CancelEventArgs>(
                        x => childWindow.Closing += x,
                        x => childWindow.Closing -= x)
                    .Subscribe(x =>
                    {
                        var vcq = new ViewModelCloseQueryArgs {IsCanceled = false};
                        vm.Closing(vcq);

                        if (vcq.IsCanceled)
                        {
                            x.EventArgs.Cancel = true;
                        }
                    })
                    .DisposeWith(vm.Disposables);
            }

            var disposables = new CompositeDisposable();
            view.ViewModel
                .WhenAnyValue(x => x.Title)
                .Subscribe(x => childWindow.Title = x)
                .DisposeWith(disposables);
            view.ViewModel
                .WhenAnyValue(x => x.CanClose)
                .Subscribe(x => childWindow.ShowCloseButton = x)
                .DisposeWith(disposables);

            Observable
                .FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                    x => childWindow.ClosingFinished += x,
                    x => childWindow.ClosingFinished -= x)
                .Select(x => (ChildWindow) x.Sender)
                .Take(1)
                .Subscribe(x =>
                {
                    disposables.Dispose();
                    vm?.Closed(new ViewModelCloseQueryArgs());
                    x.Content = null;
                });

            return Window.ShowChildWindowAsync<TResult>(
                childWindow,
                options.OverlayFillBehavior
            );
        }

        public Task<TResult> ShowChildWindowView<TView, TResult>(
            ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
            where TView : class, IView
        {
            return ShowChildWindowView<TResult>(
                container => container.Resolve<TView>(),
                viewRequest,
                options);
        }

        public async void ShowChildWindowView(Func<ILifetimeScope, IView> viewFactory, ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
        {
            await ShowChildWindowView<Unit>(viewFactory, viewRequest, options);
        }

        public void ShowChildWindowView<TView>(
            ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
            where TView : class, IView
        {
            ShowChildWindowView<TView, Unit>();
        }


        public void ShowStartView<TStartWindow, TStartView>(
            UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView
        {
            InitOnStart<TStartWindow>(options);
            ShowView<TStartView>();
            Window.Show();
        }

        public void ShowStartView<TStartWindow>(
            UiShowStartWindowOptions options = null)
            where TStartWindow : class
        {
            InitOnStart<TStartWindow>(options);
            Window.Show();
        }

        private void InitOnStart<TStartWindow>(UiShowStartWindowOptions options)
            where TStartWindow : class
        {
            if (options != null)
                Title = options.Title;

            DockingManager = options?.DockingManager ?? new DefaultDockingManager();
            IDisposable dockingManagerSubscription = null;
            Dictionary<ILayoutPane, LayoutContent> placeholders = null;
            this.WhenAnyValue(x => x.DockingManager)
                .Subscribe(dm =>
                {
                    if (placeholders != null)
                        foreach (var placeholder in placeholders.Values)
                            placeholder.Close();

                    dockingManagerSubscription?.Dispose();
                    if (dm == null) return;
                    dockingManagerSubscription = Observable
                        .FromEventPattern(
                            x => dm.ActiveContentChanged += x,
                            x => dm.ActiveContentChanged -= x)
                        .Select(x => ((DockingManager) x.Sender).ActiveContent as IView)
                        .Subscribe(x => SelectedView = x);

                    var dmDescendents = dm.Layout.Descendents().ToList();
                    placeholders = dmDescendents
                        .OfType<DependencyObject>()
                        .Where(x => !string.IsNullOrEmpty(DockContainer.GetName(x)))
                        .OfType<ILayoutPane>()
                        .ToDictionary(
                            x => x,
                            x =>
                            {
                                var content = new LayoutAnchorable();
                                switch (x)
                                {
                                    case LayoutGroup<LayoutAnchorable> la:
                                        la.Children.Add(content);
                                        break;
                                    case LayoutGroup<LayoutContent> ld:
                                        ld.Children.Add(content);
                                        break;
                                }

                                content.IsVisible = false;

                                return (LayoutContent) content;
                            });
                });

            var startObject = Container.Resolve<TStartWindow>() ??
                              throw new InvalidOperationException(
                                  $"You should register {typeof(TStartWindow)} in container");

            Window = startObject as Window ??
                     throw new InvalidCastException($"{startObject.GetType()} is not a window");
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

        private static void CloseContent(LayoutContent layout)
        {
            if (layout.Content is IView view &&
                view.ViewModel is ViewModelBase vm)
                vm.Close();
        }
    }
}