using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl.ElementsManagement
{
    internal class CategoriesManager
    {
        internal void BuildAutoCategories(
            IEnumerable cards,
            IDimension horizontalDimension,
            IDimension verticalDimension,
            PropertyAccessorsExpressionCreator propertyAccessors)
        {
            if (horizontalDimension is IDynamicDimension dynamicHorizontalDimension)
            {
                horizontalDimension.Categories = GetDimensionCategories(
                    cards,
                    dynamicHorizontalDimension,
                    propertyAccessors);
            }

            if (verticalDimension is IDynamicDimension dynamicVerticalDimension)
            {
                verticalDimension.Categories = GetDimensionCategories(
                    cards,
                    dynamicVerticalDimension,
                    propertyAccessors);
            }
        }

        private IList<IDimensionCategory> GetDimensionCategories(
            IEnumerable cards,
            IDynamicDimension dimension,
            PropertyAccessorsExpressionCreator propertyAccessors)
        {
            var getElementCategory = propertyAccessors.TakeGetterForProperty(dimension.ExpressionPath);
            var getSortingValue = propertyAccessors.TakeGetterForProperty(dimension.SortingPath);

            HashSet<object> tagFilter = null;
            if (dimension.Tags != null)
            {
                tagFilter = new HashSet<object>(dimension.Tags);
            }

            if (getElementCategory != null)
            {
                var categoriesEnumerable = cards.Cast<object>()
                    .Select(card => new
                    {
                        category = getElementCategory(card),
                        sortingElement = getSortingValue != null
                            ? getSortingValue(card)
                            : null
                    })
                    .Where(card => card.category != null)
                    .GroupBy(g => g.category)
                    .Select(g => new
                    {
                        category = g.Key,
                        sortingValue = g.First().sortingElement
                    });


                if (tagFilter != null)
                {
                    categoriesEnumerable = categoriesEnumerable
                        .Where(c => tagFilter.Contains(c.category));
                }

                if (getSortingValue != null)
                {
                    categoriesEnumerable = categoriesEnumerable
                        .OrderBy(c => c.sortingValue);
                }

                var categories = categoriesEnumerable
                    .Select(c => (IDimensionCategory)new TagsDimensionCategory(c.category.ToString(), c.category))
                    .ToList();

                return categories;
            }

            return null;
        }
    }
}
