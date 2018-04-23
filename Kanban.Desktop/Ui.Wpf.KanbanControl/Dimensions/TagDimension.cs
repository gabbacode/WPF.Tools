using System.Collections.Generic;
using System.Linq;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagDimension : BaseDimension
    {
        public TagDimension(
            object[] tags,
            IList<IDimensionCategory> categories)
        {
            Tags = tags;
            Categories = categories;
        }

        public object[] Tags { get; set; }
    }
}