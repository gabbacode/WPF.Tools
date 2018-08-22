using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public interface IDimension
    {
        string Caption { get; set; }

        string ExpressionPath { get; set; }

        string SortingPath { get; set; }

        IList<IDimensionCategory> Categories { get; set; }
    }
}
