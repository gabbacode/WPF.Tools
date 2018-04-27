using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public abstract class BaseDimension : IDimension
    {
        public string Caption { get; set; }
        
        public string ExpressionPath { get; set; }

        public virtual IList<IDimensionCategory> Categories { get; set; }
    }
}