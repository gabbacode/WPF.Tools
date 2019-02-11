using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.Views;

namespace Ui.Wpf.Demo.ViewModels
{
    public class ChildWindowDemoViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ShowChildWindowCommand { get; set; }

        public UiShowChildWindowOptions Options { get; set; } = new UiShowChildWindowOptions();

        public int ChildWindowWidth { get; set; }
        public bool ChildWindowHasWidth { get; set; }
        public int ChildWindowHeight { get; set; }
        public bool ChildWindowHasHeight { get; set; }

        public string ChildWindowViewId { get; set; }

        public string Text { get; set; } =
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Quisque odio ante, sollicitudin non purus sagittis, lobortis ultrices nulla.
Nam nulla dolor, venenatis sit amet eros quis, eleifend elementum sem.
Nulla facilisi.
Morbi non felis sed nulla fermentum accumsan faucibus sit amet elit.
Sed interdum nunc diam.
Proin pretium dolor in porta varius.
Quisque nec ligula mollis, scelerisque sem id, ultrices dolor.";

        public ChildWindowDemoViewModel(IShell shell)
        {
            ShowChildWindowCommand =
                ReactiveCommand
                    .Create(() =>
                    {
                        Options.Width = ChildWindowHasWidth ? ChildWindowWidth : (int?) null;
                        Options.Height = ChildWindowHasHeight ? ChildWindowHeight : (int?) null;
                        shell.ShowChildWindowView<TextBoxView, Unit>(new TextBoxViewRequest
                        {
                            Text = Text,
                            ViewId = ChildWindowViewId
                        }, Options);
                    })
                    .DisposeWith(Disposables);
        }
    }
}