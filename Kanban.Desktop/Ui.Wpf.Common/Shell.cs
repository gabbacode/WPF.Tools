using System;
using System.Windows;
using Autofac;
using ReactiveUI;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace Ui.Wpf.Common
{
    public class Shell : ReactiveObject
    {
        public IContainer Container { get; set; }

        //TODO replace to abstract manager
        public DockingManager DockingManager { get; private set; }
        public LayoutDocumentPane DocumentPane { get; private set; }

        public void ShowView<TView>()
        {
            var view = Container.Resolve<TView>();

            var layoutDocument = new LayoutDocument();
            layoutDocument.Content = view;
            DocumentPane.Children.Add(layoutDocument);
            layoutDocument.IsActive = true;
        }

        internal void ShowStartView<TStartWindow, TStartView>()
        {
            var startObject = Container.Resolve<TStartWindow>();

            if (startObject == null)
                throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            var window = startObject as Window;
            if (window == null)
                throw new InvalidCastException($"{startObject.GetType().ToString()} is not a window");

            ShowView<TStartView>();

            window.Show();
        }

        internal void ShowStartView<TStartWindow>()
        {
            var startObject = Container.Resolve<TStartWindow>();

            if (startObject == null)
                throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            var window = startObject as Window;
            if (window == null)
                throw new InvalidCastException($"{startObject.GetType().ToString()} is not a window");

            window.Show();
        }

        internal void AttachDockingManager(DockingManager dockingManager)
        {
            DockingManager = dockingManager;

            var layoutRoot = new LayoutRoot();
            DockingManager.Layout = layoutRoot;

            DocumentPane = layoutRoot.RootPanel.Children[0] as LayoutDocumentPane;
        }
    }
}
