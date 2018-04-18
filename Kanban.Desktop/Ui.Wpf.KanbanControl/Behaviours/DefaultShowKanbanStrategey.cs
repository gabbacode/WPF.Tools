using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl
{
    internal class DefaultShowKanbanStrategey : IShowKanbanStrategy
    {
        public DefaultShowKanbanStrategey(IKanbanBoard kanbanBoard)
        {
            KanbanBoard = kanbanBoard;
        }

        public void Show()
        {
            ClearSplitters();
            ClearDefinitions();

            BuildGridDefenitions();
            BuildGridSpliters();
        }

        public IKanbanBoard KanbanBoard { get; }

        private void BuildGridSpliters()
        {
            if (KanbanBoard.VerticalCategories.Count == 0
                || KanbanBoard.HorisontalCategories.Count == 0)
                return;

            for (int i = 0; i < KanbanBoard.VerticalCategories.Count - 1; i++)
            {
                KanbanBoard.KanbanGrid.Children.Add(
                    BuildVerticalSpliter(i, KanbanBoard.HorisontalCategories.Count));
            }

            for (int i = 0; i < KanbanBoard.HorisontalCategories.Count - 1; i++)
            {
                KanbanBoard.KanbanGrid.Children.Add(
                    BuildHorizontalSpliter(i, KanbanBoard.VerticalCategories.Count));
            }
        }

        private GridSplitter BuildVerticalSpliter(int index, int horizontalCategoriescount)
        {
            var newSpliter = new GridSplitter
            {
                ResizeDirection = GridResizeDirection.Columns,
                Width = KanbanBoard.SpliterWidth,
                Background = KanbanBoard.SpliterBackground,
            };

            Grid.SetRow(newSpliter, 0);
            Grid.SetColumn(newSpliter, index);
            Grid.SetRowSpan(newSpliter, horizontalCategoriescount);

            return newSpliter;
        }

        private GridSplitter BuildHorizontalSpliter(int index, int verticalCategoriesCount)
        {
            var newSpliter = new GridSplitter
            {
                ResizeDirection = GridResizeDirection.Rows,
                Height = KanbanBoard.SpliterWidth,
                Background = KanbanBoard.SpliterBackground,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            Grid.SetColumn(newSpliter, 0);
            Grid.SetRow(newSpliter, index);
            Grid.SetColumnSpan(newSpliter, verticalCategoriesCount);

            return newSpliter;
        }

        private void BuildGridDefenitions()
        {
            foreach (var hCategory in KanbanBoard.HorisontalCategories)
            {
                KanbanBoard.KanbanGrid.RowDefinitions.Add(new RowDefinition());
            }

            foreach (var vCategory in KanbanBoard.VerticalCategories)
            {
                KanbanBoard.KanbanGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }
        }

        private void ClearDefinitions()
        {
            KanbanBoard.KanbanGrid.RowDefinitions.Clear();
            KanbanBoard.KanbanGrid.ColumnDefinitions.Clear();
        }

        private void ClearSplitters()
        {
            var itemsToRemove = KanbanBoard.KanbanGrid.Children
                .OfType<GridSplitter>()
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
            {
                KanbanBoard.KanbanGrid.Children.Remove(itemToRemove);
            }
        }
    }
}