using System.Windows.Media;

namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public interface ICardColor
    {
        Brush BorderBrush { get; set; }

        Brush Background { get; set; }
    }
}
