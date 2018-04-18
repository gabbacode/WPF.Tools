using System.Collections.Generic;
using System.Collections.ObjectModel;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl
{
    internal class DefaultElementsDispenser : IElementsDispenser
    {
        internal void DispenceItems(
            ICollection<Card> cards, 
            ObservableCollection<IDimensionCategory> horisontalCategories, 
            ObservableCollection<IDimensionCategory> verticalCategories)
        {
            if (verticalCategories.Count == 0
                || horisontalCategories.Count == 0)
                return;

            var i = 0;
            foreach (var card in cards)
            {
                card.HorizontalCategoryIndex = i % horisontalCategories.Count;
                card.VerticalCategoryIndex = (i / horisontalCategories.Count) % verticalCategories.Count;
                i++;
            }
        }
    }
}
