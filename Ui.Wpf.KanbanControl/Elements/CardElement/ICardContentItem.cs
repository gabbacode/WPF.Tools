namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public interface ICardContentItem
    {
        string ExpressionPath { get; }

        CardContentArea Area { get; }
    }
}