using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public class TagDimension : BaseDimension, IDynamicDimension
    {
        public TagDimension(string expressionPath)
        {
            ExpressionPath = expressionPath;
        }

        public TagDimension(string expressionPath, string[] avalibleTags)
        {
            ExpressionPath = expressionPath;
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