using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ui.Wpf.KanbanControl.Elements;

namespace Ui.Wpf.KanbanControl.Behaviours
{
    internal class DefaultShowKanbanStrategey : IShowKanbanStrategy
    {
        public DefaultShowKanbanStrategey(IKanbanBoard kanbanBoard)
        {
            KanbanBoard = kanbanBoard;
            elementsDispenser = new DefaultElementsDispenser();
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
            RemoveHeaders();

            ClearSplitters();
            ClearDefinitions();

            elementsDispenser.DispenceItems(
                KanbanBoard.Cards,
                KanbanBoard.HorizontalDimension,
                KanbanBoard.VerticalDimension);

            BuildGridDefenitions();
            BuildGridSpliters();

            PlaceHeaders();
            PlaceCells();
            PlaceItems();
        }


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
            Grid.SetColumn(newSpliter, index + HeaderCellCount);
            Grid.SetRowSpan(newSpliter, verticalCategoriesCount + HeaderCellCount);

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
            Grid.SetRow(newSpliter, index + HeaderCellCount);
            Grid.SetColumnSpan(newSpliter, horizontalCategoriescount + HeaderCellCount);

            return newSpliter;
        }

        private void BuildGridDefenitions()
        {
            // header
            KanbanBoard.KanbanGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Auto
            });
            
            foreach (var hCategory in KanbanBoard.HorizontalDimension.Categories)
            {
                KanbanBoard.KanbanGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // header
            KanbanBoard.KanbanGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });
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

        private void RemoveHeaders()
        {
            var itemsToRemove = KanbanBoard.KanbanGrid.Children
                .OfType<HeaderView>()
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
            {
                KanbanBoard.KanbanGrid.Children.Remove(itemToRemove);
            }
        }
        
        private void PlaceHeaders()
        {
            for (int i = 0; i < KanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                var head = KanbanBoard.HorizontalHeaders[i].View;
                Grid.SetColumn(head, i + HeaderCellCount);
                Grid.SetRow(head, 0);
                
                KanbanBoard.KanbanGrid.Children.Add(head);
            }

            for (int j = 0; j < KanbanBoard.VerticalDimension.Categories.Count; j++)
            {
                var head = KanbanBoard.VerticalHeaders[j].View;
                Grid.SetColumn(head, 0);
                Grid.SetRow(head, j + HeaderCellCount);
                
                KanbanBoard.KanbanGrid.Children.Add(head);
            }
            
        }
        
        private void PlaceCells()
        {
            for (int i = 0; i < KanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < KanbanBoard.VerticalDimension.Categories.Count; j++)
                {
                    var cell = KanbanBoard.Cells[i, j].View;

                    Grid.SetColumn(cell, i + HeaderCellCount);
                    Grid.SetRow(cell, j + HeaderCellCount);

                    KanbanBoard.KanbanGrid.Children.Add(cell);
                }
            }
        }

        private void PlaceItems()
        {
            foreach (var card in KanbanBoard.Cards)
            {
                if (card.HorizontalCategoryIndex < 0
                    || card.VerticalCategoryIndex < 0)
                    continue;
                
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

        private const int HeaderCellCount = 1;
        
        private IKanbanBoard KanbanBoard { get; }

        private readonly DefaultElementsDispenser elementsDispenser;
    }
}