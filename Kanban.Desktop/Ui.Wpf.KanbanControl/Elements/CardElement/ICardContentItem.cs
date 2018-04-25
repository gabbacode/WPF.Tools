namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public interface ICardContentItem
    {
        string Path { get; }

        CardContentArea Area { get; }
    }
}