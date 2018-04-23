using System.Collections.Generic;
using System.Data.Odbc;
using System.Linq;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;
using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl.Behaviours
{
    internal class DefaultElementsDispenser : IElementsDispenser
    {
        public void DispenceItems(
            ICollection<Card> cards, 
            IDimension horizontalDimention,
            IDimension verticalDimension)
        {
            if (cards.Count == 0)
                return;
            
            var type = cards.First().Item.GetType();
            propertyAccessors = new PropertyAccessorsExpressionCreator(type);
            
            foreach (var card in cards)
            {
                if (horizontalDimention is IDimensionIndexSource horizontalDimentionWithIndex)
                {
                    card.HorizontalCategoryIndex = horizontalDimentionWithIndex.GetDimensionIndex(card.Item);
                }
                else
                {
                    card.HorizontalCategoryIndex = GetDimensionIndex(horizontalDimention, card.Item);
                }

                if (verticalDimension is IDimensionIndexSource verticalDimensionWithIndex)
                {
                    card.VerticalCategoryIndex = verticalDimensionWithIndex.GetDimensionIndex(card.Item);                    
                }
                else
                {
                    card.VerticalCategoryIndex = GetDimensionIndex(verticalDimension, card.Item);
                }
            }
        }
        
        public int GetDimensionIndex(IDimension dimension, object item)
        {
            var getter = propertyAccessors.TakeGetterForProperty(dimension.FieldName);
            var itemTag = getter(item);

            var categoryIndex = 0;
            // all Categories must be TagsDimensionCategory
            foreach (var category in dimension.Categories.OfType<TagsDimensionCategory>())
            {
                if (category.Tags.Contains(itemTag))
                {
                    return categoryIndex;
                }

                categoryIndex++;
            }

            return -1;
        }
        
        private PropertyAccessorsExpressionCreator propertyAccessors;
    }
}
