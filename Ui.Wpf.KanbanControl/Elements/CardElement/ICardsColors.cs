using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public interface ICardsColors
    {
        string Path { get; set; }

        Dictionary<object, ICardColor> ColorMap { get; set; }
    }
}
