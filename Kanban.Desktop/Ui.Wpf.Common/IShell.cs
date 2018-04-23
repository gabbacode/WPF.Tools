namespace Ui.Wpf.Common
{
    public interface IShell 
    {
        string Title { get; set; }

        void ShowView<TView>(UiShowOptions options = null)
            where TView : class, IView;
    }
}