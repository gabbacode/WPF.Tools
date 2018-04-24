namespace Ui.Wpf.KanbanControl.Elements
{
    public class CardContent : ICardContent
    { 
        public string[] CardContentItemsPaths { get; set; }

        public CardContent(string[] cardsPaths)
        {
            CardContentItemsPaths = cardsPaths;
        }
    }
}