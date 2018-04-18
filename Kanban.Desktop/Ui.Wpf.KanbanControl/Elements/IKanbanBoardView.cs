using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;

namespace Ui.Wpf.KanbanControl.Elements
{
    internal interface IKanbanBoard
    {
        // TODO replace to abstract methods, so we easy can use not only grid
        Grid KanbanGrid { get; }

        List<Card> Cards { get; }

        ObservableCollection<IDimensionCategory> HorisontalCategories { get; }

        ObservableCollection<IDimensionCategory> VerticalCategories { get; }

        double SpliterWidth { get; }

        Brush SpliterBackground { get; }

    }
}