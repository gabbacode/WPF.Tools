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

        public ToolsViewModel(IShell shell)
        {
            ShowMainViewCommand = ReactiveCommand.Create(() =>
            {
                shell.ShowView<MainView>(
                    new ViewRequest("main-view"),
                    new UiShowOptions {Title = nameof(MainView)}
                );
            });
        }
    }
}