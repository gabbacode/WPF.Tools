using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagsDimensionCategory<TTag> : BaseDimensionCategory
    {
        public TagsDimensionCategory()
        {
        }

        public TagsDimensionCategory(string caption, TTag tag)
        {
            Caption = caption;
            Tags = new HashSet<TTag> { tag };
        }

        public TagsDimensionCategory(IEnumerable<TTag> tags)
        {
            Tags = new HashSet<TTag>(tags);
        }

        public HashSet<TTag> Tags { get; private set; }
    }
}
