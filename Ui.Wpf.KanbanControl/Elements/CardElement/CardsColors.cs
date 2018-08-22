using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Elements.CardElement
{
    public class CardsColors : ICardsColors
    {
        public string Path { get; set; }

        public Dictionary<object, ICardColor> ColorMap { get; set; }
    }
}
