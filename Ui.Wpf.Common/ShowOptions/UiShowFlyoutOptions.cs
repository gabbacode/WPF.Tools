using System.Windows.Input;
using MahApps.Metro.Controls;

namespace Ui.Wpf.Common.ShowOptions
{
    public class UiShowFlyoutOptions : UiShowOptions
    {
        public bool IsModal { get; set; } = false;
        public Position Position { get; set; } = Position.Left;
        public FlyoutTheme Theme { get; set; } = FlyoutTheme.Dark;

        public MouseButton ExternalCloseButton { get; set; } = MouseButton.Left;
        public bool IsPinned { get; set; } = true;

        /// <summary>
        /// Can be closed with ESC
        /// </summary>
        public bool CloseButtonIsCancel { get; set; } = false;

        public bool AnimateOpacity { get; set; } = false;
        public bool AreAnimationsEnabled { get; set; } = true;

        public bool IsAutoCloseEnabled { get; set; } = false;
        public long AutoCloseInterval { get; set; } = 5000L;

        public ICommand CloseCommand { get; set; }
        public object CloseCommandParameter { get; set; }
    }
}