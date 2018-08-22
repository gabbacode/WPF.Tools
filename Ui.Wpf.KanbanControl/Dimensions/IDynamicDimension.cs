namespace Ui.Wpf.KanbanControl.Dimensions
{
    public interface IDynamicDimension : IDimension
    {
        object[] Tags { get; set; }
    }
}