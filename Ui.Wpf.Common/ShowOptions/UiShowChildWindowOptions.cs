using System.Windows.Input;

namespace Ui.Wpf.Common.ShowOptions
{
    public class UiShowChildWindowOptions : UiShowOptions
    {
        public bool IsModal { get; set; } = true;
        public bool AllowMove { get; set; } = true;
        public bool CloseOnOverlay { get; set; } = false;
        public bool CloseByEscape { get; set; } = false;
        public bool ShowTitleBar { get; set; } = true;
        public bool EnableDropShadow { get; set; } = true;

        public bool IsAutoCloseEnabled { get; set; } = false;
        public long AutoCloseInterval { get; set; } = 5000L;

        public ICommand CloseButtonCommand { get; set; }
        public object CloseButtonCommandParameter { get; set; }
    }
}