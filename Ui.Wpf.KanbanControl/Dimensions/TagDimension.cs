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
            :this(expressionPath)
        {
            Tags = avalibleTags;
        }

        public TagDimension(string expressionPath, string sortingPath, string[] avalibleTags)
            :this(expressionPath, avalibleTags)
        {
            SortingPath = sortingPath;
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