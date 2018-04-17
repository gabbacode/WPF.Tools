using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ui.Wpf.KanbanControl
{
    internal interface IKanbanBoardView
    {
        Grid KanbanGrid { get; }

        ObservableCollection<IDimensionCategory> HorisontalCategories { get; }

        ObservableCollection<IDimensionCategory> VerticalCategories { get; }

        double SpliterWidth { get; }

        Brush SpliterBackground { get; }
    }
}