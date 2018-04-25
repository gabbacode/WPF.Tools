using System.Windows.Media;

namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public class CardColor : ICardColor
    {
        public Brush BorderBrush { get; set; }

        public Brush Background { get; set; }
    }
}
