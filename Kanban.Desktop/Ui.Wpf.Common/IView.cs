using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Common
{
    public interface IView
    {
        IViewModel ViewModel { get; set; }

        void Configure(UiShowOptions options);
    }
}