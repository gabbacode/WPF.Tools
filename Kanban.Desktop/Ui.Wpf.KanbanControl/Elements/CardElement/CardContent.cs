namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public class CardContent : ICardContent
    { 
        public ICardContentItem[] CardContentItems { get; set; }

        public CardContent(ICardContentItem[] cardContentItems)
        {
            CardContentItems = cardContentItems;
        }
    }
}