using System.Collections.Generic;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl
{
    internal class DefaultElementsDispenser : IElementsDispenser
    {
        internal void DispenceItems(
            ICollection<Card> cards, 
            IDimension horizontalDimention,
            IDimension verticalDimension)
        {
            foreach (var card in cards)
            {
                card.HorizontalCategoryIndex = horizontalDimention.GetDimensionIndex(card.Item);
                card.VerticalCategoryIndex = verticalDimension.GetDimensionIndex(card.Item);
            }
        }
    }
}
