using ReactiveUI;
using System.Reactive;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.Views;

namespace Ui.Wpf.Demo.ViewModels
{
    public class ToolsViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ShowMainViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ShowFlyoutDemoViewCommand { get; set; }
        public ReactiveCommand<Unit, Unit> ShowChildWindowViewCommand { get; set; }

        public ToolsViewModel(IShell shell)
        {
            ShowMainViewCommand = ReactiveCommand.Create(() =>
            {
                shell.ShowView<MainView>(
                    new ViewRequest("main-view"),
                    new UiShowOptions {Title = nameof(MainView)}
                );
            });
            ShowFlyoutDemoViewCommand = ReactiveCommand.Create(() =>
            {
                shell.ShowView<FlyoutDemoView>(
                    new ViewRequest("flyout-demo-view"),
                    new UiShowOptions { Title = nameof(FlyoutDemoView) }
                );
            });
            ShowChildWindowViewCommand = ReactiveCommand.Create(() =>
            {
                shell.ShowView<ChildWindowDemoView>(
                    new ViewRequest("child-window-demo-view"),
                    new UiShowOptions { Title = nameof(FlyoutDemoView) }
                );
            });
        }
    }
}