using System.Collections.Generic;

namespace Ui.Wpf.KanbanControl.Dimensions
{
    public abstract class BaseDimension : IDimension
    {
        public string Caption { get; set; }

        public IList<IDimensionCategory> Categories { get; protected set; }

        public abstract int GetDimensionIndex(object item);
    }
}