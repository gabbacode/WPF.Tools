namespace Ui.Wpf.Common.ViewModels
{
    public interface IResultableViewModel<TResult>
    {
        TResult ViewResult { get; set; }
    }
}