using System.Windows.Input;
using MahApps.Metro.Controls;

namespace Ui.Wpf.Common.ShowOptions
{
    public class UiShowFlyoutOptions : UiShowOptions
    {
        public Position Position { get; set; } = Position.Left;
        public bool IsModal { get; set; } = false;
        public bool IsPinned { get; set; } = true;
        public FlyoutTheme Theme { get; set; } = FlyoutTheme.Dark;

        /// <summary>
        /// Can be closed with ESC
        /// </summary>
        public bool CloseButtonIsCancel { get; set; } = false;
        
        public ICommand CloseCommand { get; set; }
        public object CloseCommandParameter { get; set; }
    }
}