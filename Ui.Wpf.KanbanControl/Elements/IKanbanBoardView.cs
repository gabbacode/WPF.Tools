using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Ui.Wpf.KanbanControl.Elements
{
    internal interface IKanbanBoard
    {
        // TODO replace to abstract methods, so we easy can use not only grid
        Grid KanbanGrid { get; }

        List<Header> HorizontalHeaders { get; }
 
        List<Header> VerticalHeaders { get; }

        List<Card> CardElements { get; }

        Cell[,] Cells { get; }

        IDimension HorizontalDimension { get; }

        IDimension VerticalDimension { get; }

        double SpliterWidth { get; }

        Brush SpliterBackground { get; }
    }
}