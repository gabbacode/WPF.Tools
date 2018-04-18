using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ui.Wpf.KanbanControl.Elements
{
    internal interface IKanbanBoard
    {
        Grid KanbanGrid { get; }

        ObservableCollection<IDimensionCategory> HorisontalCategories { get; }

        ObservableCollection<IDimensionCategory> VerticalCategories { get; }

        double SpliterWidth { get; }

        Brush SpliterBackground { get; }
    }
}