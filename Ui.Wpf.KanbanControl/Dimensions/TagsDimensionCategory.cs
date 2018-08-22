using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagsDimensionCategory : BaseDimensionCategory
    {
        public TagsDimensionCategory()
        {
        }

        public TagsDimensionCategory(string caption, object tag)
        {
            Caption = caption;
            Tags = new HashSet<object> { tag };
        }

        public TagsDimensionCategory(IEnumerable<object> tags)
        {
            Tags = new HashSet<object>(tags);
        }

        public HashSet<object> Tags { get; private set; }
    }
}