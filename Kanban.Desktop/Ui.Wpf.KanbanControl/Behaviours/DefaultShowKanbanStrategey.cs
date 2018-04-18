using System;
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

            RemoveItems();
            RemoveCells();

            ClearSplitters();
            ClearDefinitions();

            ElementsDispenser.DispenceItems(
                KanbanBoard.Cards,
                KanbanBoard.HorizontalDimension,
                KanbanBoard.VerticalDimension);

            BuildGridDefenitions();
            BuildGridSpliters();

            PlaceCells();
            PlaceItems();
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

        private void PlaceCells()
        {
            for (int i = 0; i < KanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < KanbanBoard.VerticalDimension.Categories.Count; j++)
                {
                    var cell = KanbanBoard.Cells[i, j].View;

                    Grid.SetColumn(cell, i);
                    Grid.SetRow(cell, j);

                    KanbanBoard.KanbanGrid.Children.Add(cell);
                }
            }
        }

        private void PlaceItems()
        {
            foreach (var card in KanbanBoard.Cards)
            {
                KanbanBoard.Cells[card.HorizontalCategoryIndex, card.VerticalCategoryIndex].View.ItemContainer.Children.Add(card.View);
            }
        }

        private void RemoveItems()
        {
            for (int i = 0; i < KanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < KanbanBoard.VerticalDimension.Categories.Count; j++)
                {
                    KanbanBoard.Cells[i, j].View.ItemContainer.Children.Clear();
                }
            }
        }

        private void RemoveCells()
        {
            var toRemoveItems = KanbanBoard.KanbanGrid.Children
                .OfType<CellView>()
                .ToArray();

            foreach (var toRemove in toRemoveItems)
            {
                KanbanBoard.KanbanGrid.Children.Remove(toRemove);
            }
        }

        private DefaultElementsDispenser ElementsDispenser;
    }
}