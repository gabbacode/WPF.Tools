using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagDimension : BaseDimension, IDynamicDimension
    {
        public TagDimension(string fieldName)
        {
            FieldName = fieldName;
        }

        public TagDimension(string fieldName, string[] avalibleTags)
        {
            FieldName = fieldName;
            Tags = avalibleTags;
        }

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