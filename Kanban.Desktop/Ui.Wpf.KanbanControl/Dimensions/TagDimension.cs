using System;
using System.Collections.Generic;
using System.Linq;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagDimension<TTag, TElement> : BaseDimension
    {
        public TagDimension(
            TTag[] tags, 
            Func<TElement, ICollection<TTag>> getItemTags,
            IList<IDimensionCategory> categories)
        {
            Tags = tags;
            this.getItemTags = getItemTags;
            Categories = categories;
        }

        public override int GetDimensionIndex(object item)
        {
            var itemTags = getItemTags((TElement)item);

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

        private readonly Func<TElement, ICollection<TTag>> getItemTags;
    }
}
