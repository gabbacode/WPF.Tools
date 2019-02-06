using Autofac;
using MahApps.Metro.Controls;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Xceed.Wpf.AvalonDock;

namespace Ui.Wpf.Common
{
    public interface IShell 
    {
        string Title { get; set; }

        IContainer Container { get; set; }

        IView SelectedView { get; set; } 

        void ShowView<TView>(
            ViewRequest viewRequest = null, 
            UiShowOptions options = null
            )
            where TView : class, IView;

        void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView;

        void ShowFlyoutView<TView>(
            ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
            where TView : class, IView;

        void ShowStartView<TStartWindow>(UiShowStartWindowOptions options = null)
            where TStartWindow : class;

        void ShowStartView<TStartWindow, TStartView>(UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView;

        void AttachDockingManager(DockingManager dockingManager);
        void AttachFlyoutsControl(FlyoutsControl flyoutsControl);

        CommandItem AddGlobalCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm, bool addSeparator = false);
        CommandItem AddVMCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm, bool addSeparator = false);
        CommandItem AddInstanceCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm);
        void RemoveCommand(CommandItem ci);
    }
}