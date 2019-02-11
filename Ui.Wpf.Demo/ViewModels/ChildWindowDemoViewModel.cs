using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.Views;

namespace Ui.Wpf.Demo.ViewModels
{
    public class ChildWindowDemoViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ShowChildWindowCommand { get; set; }

        public ChildWindowDemoViewModel(IShell shell)
        {
            ShowChildWindowCommand =
                ReactiveCommand
                    .Create(() =>
                    {
                        shell.ShowChildWindowView<TextBoxView, Unit>(new TextBoxViewRequest
                        {
                            Text = "Hello World!"
                        });
                    })
                    .DisposeWith(Disposables);
        }
    }
}