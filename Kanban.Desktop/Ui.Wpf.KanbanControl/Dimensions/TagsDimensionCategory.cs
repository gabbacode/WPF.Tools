using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagsDimensionCategory<TTag> : BaseDimensionCategory
    {
        public TagsDimensionCategory()
        {
        }

        public TagsDimensionCategory(TTag tag)
        {
            Tags = new HashSet<TTag> { tag };
        }

        public TagsDimensionCategory(IEnumerable<TTag> tags)
        {
            Tags = new HashSet<TTag>(tags);
        }

        public HashSet<TTag> Tags { get; private set; }
    }
}
