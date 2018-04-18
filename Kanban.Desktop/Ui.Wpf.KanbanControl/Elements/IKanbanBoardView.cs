using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Media;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Ui.Wpf.KanbanControl.Elements
{
    internal interface IKanbanBoard
    {
        // TODO replace to abstract methods, so we easy can use not only grid
        Grid KanbanGrid { get; }

        List<Card> Cards { get; }

        Cell[,] Cells { get; }

        IDimension HorizontalDimension { get; }

        IDimension VerticalDimension { get; }

        double SpliterWidth { get; }

        Brush SpliterBackground { get; }

    }
}