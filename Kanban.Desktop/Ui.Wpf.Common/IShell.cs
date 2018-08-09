using Autofac;
using Ui.Wpf.Common.ShowOptions;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;
using System;

namespace Ui.Wpf.Common
{
    public interface IShell 
    {
        string Title { get; set; }

        IContainer Container { get; set; }

        void ShowView<TView>(
            ViewRequest viewRequest = null, 
            UiShowOptions options = null,
            string viewId=null,
            Action<string> closeFunc=null
            )
            where TView : class, IView;

        void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView;

        void ShowStartView<TStartWindow>(UiShowStartWindowOptions options = null)
            where TStartWindow : class;

        void ShowStartView<TStartWindow, TStartView>(UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView;

        void AttachDockingManager(DockingManager dockingManager);

    }
}