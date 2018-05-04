namespace Ui.Wpf.Common
{
    public interface IShell 
    {
        string Title { get; set; }

        void ShowView<TView>(
            ViewRequest viewRequest = null, 
            UiShowOptions options = null)
            where TView : class, IView;

        void ShowTool<TToolView>(
            ViewRequest viewRequest = null,
            UiShowOptions options = null)
            where TToolView : class, IToolView;
    }
}