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

        public void ShowView<TView>(UiShowOptions options = null)
            where TView : class, IView 
        {
            var view = Container.Resolve<TView>();
            if (options != null)
                view.Configure(options);

            (view.ViewModel as IInitializableViewModel)?.Initialize();


            var layoutDocument = new LayoutDocument();
            layoutDocument.Content = view;

            view.ViewModel
                .WhenAnyValue(vm => vm.Title)
                .Subscribe(x => layoutDocument.Title = x);
            
            DocumentPane.Children.Add(layoutDocument);
            layoutDocument.IsActive = true;
        }

        internal void ShowStartView<TStartWindow, TStartView>()
            where TStartWindow : class
            where TStartView : class, IView
        {
            var startObject = Container.Resolve<TStartWindow>();

            if (startObject == null)
                throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            var window = startObject as Window;
            if (window == null)
                throw new InvalidCastException($"{startObject.GetType()} is not a window");

            ShowView<TStartView>();

            window.Show();
        }

        internal void ShowStartView<TStartWindow>()
            where TStartWindow : class 
        {
            var startObject = Container.Resolve<TStartWindow>();

            if (startObject == null)
                throw new InvalidOperationException($"You shuld configurate {typeof(TStartWindow)}");

            var window = startObject as Window;
            if (window == null)
                throw new InvalidCastException($"{startObject.GetType()} is not a window");

            window.Show();
        }

        internal void AttachDockingManager(DockingManager dockingManager)
        {
            DockingManager = dockingManager;

            var layoutRoot = new LayoutRoot();
            DockingManager.Layout = layoutRoot;

            DocumentPane = layoutRoot.RootPanel.Children[0] as LayoutDocumentPane;
        }
        
        //TODO replace to abstract manager
        private DockingManager DockingManager { get; set; }

        private LayoutDocumentPane DocumentPane { get; set; }
    }
}
