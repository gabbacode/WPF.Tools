using System;
using System.Collections.Generic;
using System.Linq;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagDimension<TTag, TElement> : BaseDimension
    {
        public TagDimension(
            TTag[] tags, 
            Func<TElement, IEnumerable<TTag>> getItemTags,
            IDimensionCategory[] categories)
        {
            Tags = tags;
            GetItemTags = getItemTags;
            Categories = categories;
        }

        public override int GetDimensionIndex(object item)
        {
            var itemTags = GetItemTags((TElement)item);

            var categoryIndex = 0;
            // all Categories must be TagsDimensionCategory
            foreach (var category in Categories.OfType<TagsDimensionCategory<TTag>>())
            {
                if (itemTags.Any(
                    category.Tags.Contains))
                {
                    return categoryIndex;
                }

                categoryIndex++;
            }

            return -1;
        }

        public TTag[] Tags { get; set; }

        private Func<TElement, IEnumerable<TTag>> GetItemTags;
    }
}
