using System;
using System.Windows;
using Autofac;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common.ViewModels;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace Ui.Wpf.Common
{
    public class Shell : ReactiveObject, IShell
    {
        public IContainer Container { get; set; }

        [Reactive] public string Title { get; set; }

        public void ShowView<TView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView 
        {
            var view = Container.Resolve<TView>();
            if (options != null)
                view.Configure(options);

            (view.ViewModel as IInitializableViewModel)?.Initialize(viewRequest);


            var layoutDocument = new LayoutDocument();
            layoutDocument.Content = view;

            view.ViewModel
                .WhenAnyValue(vm => vm.Title)
                .Subscribe(x => layoutDocument.Title = x);
            
            DocumentPane.Children.Add(layoutDocument);
            layoutDocument.IsActive = true;
        }

        public void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView
        {
            var view = Container.Resolve<TToolView>();
            if (options != null)
                view.Configure(options);

            (view.ViewModel as IInitializableViewModel)?.Initialize(viewRequest);

            var layoutAnchorable = new LayoutAnchorable();
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

        //TODO replace to abstract manager
        private DockingManager DockingManager { get; set; }

        private LayoutDocumentPane DocumentPane { get; set; }

        private LayoutAnchorablePane ToolsPane { get; set; }

        private int? ToolPaneWidth { get; set; }
    }
}
