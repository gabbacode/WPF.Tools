namespace Ui.Wpf.Common.ViewModels
{
    public interface IViewModel
    {
        string Title { get; set; }

        string FullTitle { get; set; }

        bool IsEnabled { get; set; }

        bool CanHide { get; set; }

        bool CanClose { get; set; } 
    }
}