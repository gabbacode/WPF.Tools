namespace Ui.Wpf.KanbanControl.Dimensions
{
    public abstract class BaseDimensionCategory : IDimensionCategory
    {
        public string Caption { get; set; }

        public double Weight { get; set; }
    }
}
