using System;
using System.Reactive.Linq;
using System.Windows;
using Autofac;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using System.Linq;

namespace Ui.Wpf.Common
{
    public class Shell : ReactiveObject, IShell
    {
        public IContainer Container { get; set; }

        [Reactive] public string Title { get; set; }

        [Reactive] public IView SelectedView { get; set; }

        public void ShowView<TView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView
        {
            LayoutContent layoutContent = null;
            if (viewRequest?.ViewId != null)
            {
                layoutContent = DocumentPane.Children.FirstOrDefault(x => x.ContentId == viewRequest?.ViewId);
            }

            if (layoutContent != null)
            {
                layoutContent.IsActive = true;
            }
            else
            {
                var view = Container.Resolve<TView>();
                if (options != null)
                    view.Configure(options);

                var layoutDocument = new LayoutDocument { Content = view };
                layoutDocument.ContentId = viewRequest?.ViewId;
                if (options != null)
                    layoutDocument.CanClose = options.CanClose;

                AddTitleRefreshing(view, layoutDocument);
                AddWindowBehaviour(view, layoutDocument);
                AddClosingByRequest(view, layoutDocument);

                DocumentPane.Children.Add(layoutDocument);

                if (view.ViewModel is ViewModelBase vmb)
                {
                    vmb.ViewId = viewRequest?.ViewId;
                }
                if (view.ViewModel is IInitializableViewModel initializibleViewModel)
                {
                    initializibleViewModel.Initialize(viewRequest);
                }

                layoutDocument.IsActive = true;
            }
        }

        public void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView
        {
            var view = Container.Resolve<TToolView>();
            if (options != null)
                view.Configure(options);

            var layoutAnchorable = new LayoutAnchorable
            {
                CanAutoHide = false,
                CanFloat = false,
            };
            view.ViewModel.CanClose = false;
            view.ViewModel.CanHide = false;

            (view.ViewModel as IInitializableViewModel)?.Initialize(viewRequest);


            AddTitleRefreshing(view, layoutAnchorable);
            AddWindowBehaviour(view, layoutAnchorable);


            layoutAnchorable.Content = view;
            ToolsPane.Children.Add(layoutAnchorable);
        }

        public void ShowStartView<TStartWindow, TStartView>(
            UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView
        {
            if (options != null)
                ToolPaneWidth = options.ToolPaneWidth;

            var startObject = Container.Resolve<TStartWindow>();

            if (startObject == null)
                throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            var window = startObject as Window;
            if (window == null)
                throw new InvalidCastException($"{startObject.GetType()} is not a window");

            ShowView<TStartView>();

            window.Show();
        }

        public void ShowStartView<TStartWindow>(
            UiShowStartWindowOptions options = null)
            where TStartWindow : class
        {
            if (options != null)
                ToolPaneWidth = options.ToolPaneWidth;

            var startObject = Container.Resolve<TStartWindow>();

            if (startObject == null)
                throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            var window = startObject as Window;
            if (window == null)
                throw new InvalidCastException($"{startObject.GetType()} is not a window");

            window.Show();
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

        private static void AddClosingByRequest<TView>(TView view, LayoutDocument layoutDocument)
            where TView : class, IView
        {
            if (view.ViewModel is ViewModelBase baseViewModel)
            {
                var closeQuery = Observable.FromEventPattern<ViewModelCloseQueryArgs>(
                    x => baseViewModel.CloseQuery += x,
                    x => baseViewModel.CloseQuery -= x);

                var subscription = closeQuery.Subscribe(x => { layoutDocument.Close(); });


                layoutDocument.Closing += (s, e) =>
                {
                    ViewModelCloseQueryArgs vcq = new ViewModelCloseQueryArgs { IsCanceled = false };
                    baseViewModel.Closing(vcq);

                    if (vcq.IsCanceled)
                    {
                       e.Cancel = true;
                    }
                       
                };


                layoutDocument.Closed += (s, e) =>
                {
                    try
                    {
                       baseViewModel.Closed(new ViewModelCloseQueryArgs { IsCanceled = false });
                    }
                    finally
                    {
                        subscription.Dispose();
                    }
                };
            }
        }

        private static void AddTitleRefreshing<TView>(TView view, LayoutContent layoutDocument)
            where TView : class, IView
        {
            var titleRefreshSubsription = view.ViewModel
                .WhenAnyValue(vm => vm.Title)
                .Subscribe(x => layoutDocument.Title = x);

            layoutDocument.Closed += (s, e) => titleRefreshSubsription.Dispose();
        }

        private static void AddWindowBehaviour<TView>(TView view, LayoutContent layoutContent)
            where TView : class, IView
        {

            if (view.ViewModel is ViewModelBase baseViewModel)
            {
                baseViewModel.Disposables.Add(
                    baseViewModel
                        .WhenAnyValue(x => x.IsEnabled)
                        .Subscribe(x => layoutContent.IsEnabled = x));

                baseViewModel.Disposables.Add(
                    baseViewModel
                        .WhenAnyValue(x => x.CanClose)
                        .Subscribe(x => layoutContent.CanClose = x));

                if (layoutContent is LayoutAnchorable layoutAnchorable)
                {
                    baseViewModel.Disposables.Add(
                        baseViewModel
                            .WhenAnyValue(x => x.CanHide)
                            .Subscribe(x => layoutAnchorable.CanHide = x));
                }
            }
        }


        //TODO replace to abstract manager
        private DockingManager DockingManager { get; set; }

        protected LayoutDocumentPane DocumentPane { get; set; }

        private LayoutAnchorablePane ToolsPane { get; set; }

        private int? ToolPaneWidth { get; set; }
    }
}
