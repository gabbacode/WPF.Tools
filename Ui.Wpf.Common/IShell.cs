using Autofac;
using MahApps.Metro.Controls;
using System.Threading.Tasks;
using System.Windows;
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

        void SetContainerWidth(string containerName, GridLength width);
        void SetContainerHeight(string containerName, GridLength height);
        void SetContainerSize(string containerName, GridLength width, GridLength height);

        void ShowView<TView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView;

        void ShowViewIn<TView>(
            string containerName,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TView : class, IView;

        void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView;

        void ShowToolIn<TToolView>(
            string containerName,
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView;

        Task<TResult> ShowFlyoutView<TView, TResult>(
            ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
            where TView : class, IView;

        void ShowFlyoutView<TView>(
            ViewRequest viewRequest = null,
            UiShowFlyoutOptions options = null)
            where TView : class, IView;

        void ShowChildWindowView<TView>(
            ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
            where TView : class, IView;

        Task<TResult> ShowChildWindowView<TView, TResult>(
            ViewRequest viewRequest = null,
            UiShowChildWindowOptions options = null)
            where TView : class, IView;

        void ShowStartView<TStartWindow>(UiShowStartWindowOptions options = null)
            where TStartWindow : class;

        void ShowStartView<TStartWindow, TStartView>(UiShowStartWindowOptions options = null)
            where TStartWindow : class
            where TStartView : class, IView;

        DockingManager DockingManager { get; set; }
        FlyoutsControl FlyoutsControl { get; set; }

        CommandItem AddGlobalCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm,
            bool addSeparator = false);

        CommandItem AddVMCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm,
            bool addSeparator = false);

        CommandItem AddInstanceCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm);
        void RemoveCommand(CommandItem ci);
    }
}