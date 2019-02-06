using MahApps.Metro.Controls;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Windows.Input;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Demo.Views;

namespace Ui.Wpf.Demo.ViewModels
{
    public class FlyoutDemoViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ShowFlyoutCommand { get; set; }
        public UiShowFlyoutOptions Options { get; set; } = new UiShowFlyoutOptions();

        public Position[] PositionSource { get; set; } =
            Enum.GetValues(typeof(Position)).Cast<Position>().ToArray();

        public FlyoutTheme[] FlyoutThemeSource { get; set; } =
            Enum.GetValues(typeof(FlyoutTheme)).Cast<FlyoutTheme>().ToArray();

        public MouseButton[] ExternalCloseButtonSource { get; set; } =
            Enum.GetValues(typeof(MouseButton)).Cast<MouseButton>().ToArray();

        public int FlyoutWidth { get; set; }
        public bool FlyoutHasWidth { get; set; }
        public int FlyoutHeight { get; set; }
        public bool FlyoutHasHeight { get; set; }

        public string Text { get; set; } =
            @"Lorem ipsum dolor sit amet, consectetur adipiscing elit.
Quisque odio ante, sollicitudin non purus sagittis, lobortis ultrices nulla.
Nam nulla dolor, venenatis sit amet eros quis, eleifend elementum sem.
Nulla facilisi.
Morbi non felis sed nulla fermentum accumsan faucibus sit amet elit.
Sed interdum nunc diam.
Proin pretium dolor in porta varius.
Quisque nec ligula mollis, scelerisque sem id, ultrices dolor.";

        public FlyoutDemoViewModel(IShell shell)
        {
            ShowFlyoutCommand =
                ReactiveCommand
                    .Create(() =>
                    {
                        Options.Width = FlyoutHasWidth ? FlyoutWidth : (int?) null;
                        Options.Height = FlyoutHasHeight ? FlyoutHeight : (int?) null;
                        shell.ShowFlyoutView<TextBoxView>(new TextBoxViewRequest(Text), Options);
                    })
                    .DisposeWith(Disposables);
        }
    }
}