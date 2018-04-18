using System.Collections.Generic;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl.Behaviours
{
    internal interface IElementsDispenser
    {
        void DispenceItems(
            ICollection<Card> cards,
            IDimension horizontalDimention,
            IDimension verticalDimension);
    }
}
