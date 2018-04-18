using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ui.Wpf.KanbanControl.Behaviours;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl
{
    internal class DefaultShowKanbanStrategey : IShowKanbanStrategy
    {
        public DefaultShowKanbanStrategey(IKanbanBoard kanbanBoard)
        {
            KanbanBoard = kanbanBoard;
            ElementsDispenser = new DefaultElementsDispenser();
        }

        public void AddActionsToShow(KanbanChangeObjectType changeObjectType)
        {
            if (KanbanBoard.HorizontalDimension == null
                || KanbanBoard.HorizontalDimension.Categories.Count == 0
                || KanbanBoard.VerticalDimension == null
                || KanbanBoard.VerticalDimension.Categories.Count == 0)
                return;

            ClearItems();
            ClearSplitters();
            ClearDefinitions();

            ElementsDispenser.DispenceItems(
                KanbanBoard.Cards,
                KanbanBoard.HorizontalDimension,
                KanbanBoard.VerticalDimension);

            BuildGridDefenitions();
            BuildGridSpliters();
            BuildItems();
        }

        public IKanbanBoard KanbanBoard { get; }

        private void BuildGridSpliters()
        {
            if (KanbanBoard.VerticalDimension.Categories.Count == 0
                || KanbanBoard.HorizontalDimension.Categories.Count == 0)
                return;

            for (int i = 0; i < KanbanBoard.VerticalDimension.Categories.Count - 1; i++)
            {
                KanbanBoard.KanbanGrid.Children.Add(
                    BuildHorizontalSpliter(i, KanbanBoard.HorizontalDimension.Categories.Count));
            }

            for (int i = 0; i < KanbanBoard.HorizontalDimension.Categories.Count - 1; i++)
            {
                KanbanBoard.KanbanGrid.Children.Add(
                    BuildVerticalSpliter(i, KanbanBoard.VerticalDimension.Categories.Count));
            }
        }

        private GridSplitter BuildVerticalSpliter(int index, int verticalCategoriesCount)
        {
            var newSpliter = new GridSplitter
            {
                ResizeDirection = GridResizeDirection.Columns,
                Width = KanbanBoard.SpliterWidth,
                Background = KanbanBoard.SpliterBackground,
            };

            Grid.SetRow(newSpliter, 0);
            Grid.SetColumn(newSpliter, index);
            Grid.SetRowSpan(newSpliter, verticalCategoriesCount);

            return newSpliter;
        }

        private GridSplitter BuildHorizontalSpliter(int index, int horizontalCategoriescount)
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
            Grid.SetColumnSpan(newSpliter, horizontalCategoriescount);

            return newSpliter;
        }

        private void BuildGridDefenitions()
        {
            foreach (var hCategory in KanbanBoard.HorizontalDimension.Categories)
            {
                KanbanBoard.KanbanGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            foreach (var vCategory in KanbanBoard.VerticalDimension.Categories)
            {
                KanbanBoard.KanbanGrid.RowDefinitions.Add(new RowDefinition());
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

        private void BuildItems()
        {
            foreach (var card in KanbanBoard.Cards)
            {
                KanbanBoard.KanbanGrid.Children.Add(card.View);
                Grid.SetColumn(card.View, card.HorizontalCategoryIndex);
                Grid.SetRow(card.View, card.VerticalCategoryIndex);
            }
        }

        private void ClearItems()
        {
            foreach (var card in KanbanBoard.Cards)
            {
                KanbanBoard.KanbanGrid.Children.Remove(card.View);
            }
        }

        private DefaultElementsDispenser ElementsDispenser;
    }
}