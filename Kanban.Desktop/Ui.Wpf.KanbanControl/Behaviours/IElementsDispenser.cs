using System.Collections.Generic;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;
using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl.Behaviours
{
    internal interface IElementsDispenser
    {
        void DispenceItems(
            ICollection<Card> cards,
            IDimension horizontalDimention,
            IDimension verticalDimension,
            PropertyAccessorsExpressionCreator propertyAccessors);
    }
}
