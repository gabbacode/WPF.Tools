using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Ui.Wpf.KanbanControl.Elements;
using Ui.Wpf.KanbanControl.Expressions;

namespace Ui.Wpf.KanbanControl.Behaviours
{
    internal class DefaultShowKanbanStrategey : IShowKanbanStrategy
    {
        public DefaultShowKanbanStrategey(IKanbanBoard kanbanBoard)
        {
            this.kanbanBoard = kanbanBoard;
            elementsDispenser = new DefaultElementsDispenser();
        }

        public void AddActionsToShow(
            KanbanChangeObjectType changeObjectType, 
            PropertyAccessorsExpressionCreator propertyAccessors)
        {
            if (kanbanBoard.HorizontalDimension == null
                || kanbanBoard.HorizontalDimension.Categories.Count == 0
                || kanbanBoard.VerticalDimension == null
                || kanbanBoard.VerticalDimension.Categories.Count == 0)
                return;

            RemoveItems();
            RemoveCells();
            RemoveHeaders();

            ClearSplitters();
            ClearDefinitions();

            elementsDispenser.DispenceItems(
                 kanbanBoard.CardElements,
                 kanbanBoard.HorizontalDimension,
                 kanbanBoard.VerticalDimension,
                 propertyAccessors);

            PlaceItems();


            BuildGridDefenitions();
            BuildGridSpliters();

            PlaceHeaders();
            PlaceCells();
        }


        private void BuildGridSpliters()
        {
            if (kanbanBoard.VerticalDimension.Categories.Count == 0
                || kanbanBoard.HorizontalDimension.Categories.Count == 0)
                return;

            for (int i = 0; i < kanbanBoard.VerticalDimension.Categories.Count - 1; i++)
            {
                kanbanBoard.KanbanGrid.Children.Add(
                    BuildHorizontalSpliter(i, kanbanBoard.HorizontalDimension.Categories.Count));
            }

            for (int i = 0; i < kanbanBoard.HorizontalDimension.Categories.Count - 1; i++)
            {
                kanbanBoard.KanbanGrid.Children.Add(
                    BuildVerticalSpliter(i, kanbanBoard.VerticalDimension.Categories.Count));
            }
        }

        private GridSplitter BuildVerticalSpliter(int index, int verticalCategoriesCount)
        {
            var newSpliter = new GridSplitter
            {
                ResizeDirection = GridResizeDirection.Columns,
                Width = kanbanBoard.SpliterWidth,
                Background = kanbanBoard.SpliterBackground,
            };

            Panel.SetZIndex(newSpliter, int.MaxValue);
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
                Height = kanbanBoard.SpliterWidth,
                Background = kanbanBoard.SpliterBackground,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            Panel.SetZIndex(newSpliter, int.MaxValue);
            Grid.SetColumn(newSpliter, 0);
            Grid.SetRow(newSpliter, index + HeaderCellCount);
            Grid.SetColumnSpan(newSpliter, horizontalCategoriescount + HeaderCellCount);

            return newSpliter;
        }

        private void BuildGridDefenitions()
        {
            // header
            kanbanBoard.KanbanGrid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Auto
            });
            foreach (var hCategory in kanbanBoard.HorizontalDimension.Categories)
            {
                kanbanBoard.KanbanGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // header
            kanbanBoard.KanbanGrid.RowDefinitions.Add(new RowDefinition()
            {
                Height = GridLength.Auto
            });
            foreach (var vCategory in kanbanBoard.VerticalDimension.Categories)
            {
                kanbanBoard.KanbanGrid.RowDefinitions.Add(new RowDefinition
                {
                    Height = GridLength.Auto
                });
            }
        }

        private void ClearDefinitions()
        {
            kanbanBoard.KanbanGrid.RowDefinitions.Clear();
            kanbanBoard.KanbanGrid.ColumnDefinitions.Clear();
        }

        private void ClearSplitters()
        {
            var itemsToRemove = kanbanBoard.KanbanGrid.Children
                .OfType<GridSplitter>()
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
            {
                kanbanBoard.KanbanGrid.Children.Remove(itemToRemove);
            }
        }

        private void RemoveHeaders()
        {
            var itemsToRemove = kanbanBoard.KanbanGrid.Children
                .OfType<HeaderView>()
                .ToArray();

            foreach (var itemToRemove in itemsToRemove)
            {
                kanbanBoard.KanbanGrid.Children.Remove(itemToRemove);
            }
        }
        
        private void PlaceHeaders()
        {
            for (int i = 0; i < kanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                var head = kanbanBoard.HorizontalHeaders[i].View;
                Grid.SetColumn(head, i + HeaderCellCount);
                Grid.SetRow(head, 0);
                
                kanbanBoard.KanbanGrid.Children.Add(head);
            }

            for (int j = 0; j < kanbanBoard.VerticalDimension.Categories.Count; j++)
            {
                var head = kanbanBoard.VerticalHeaders[j].View;
                Grid.SetColumn(head, 0);
                Grid.SetRow(head, j + HeaderCellCount);
                
                kanbanBoard.KanbanGrid.Children.Add(head);
            }
            
        }
        
        private void PlaceCells()
        {
            for (int i = 0; i < kanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < kanbanBoard.VerticalDimension.Categories.Count; j++)
                {
                    var cell = kanbanBoard.Cells[i, j].View;

                    Grid.SetColumn(cell, i + HeaderCellCount);
                    Grid.SetRow(cell, j + HeaderCellCount);

                    kanbanBoard.KanbanGrid.Children.Add(cell);
                }
            }
        }

        private void PlaceItems()
        {
            foreach (var card in kanbanBoard.CardElements)
            {
                if (card.HorizontalCategoryIndex < 0
                    || card.VerticalCategoryIndex < 0)
                    continue;

                kanbanBoard.Cells[card.HorizontalCategoryIndex, card.VerticalCategoryIndex].ItemsCount++;
                kanbanBoard.Cells[card.HorizontalCategoryIndex, card.VerticalCategoryIndex].View.ItemContainer.Children.Add(card.View);
            }
        }

        private void RemoveItems()
        {
            for (int i = 0; i < kanbanBoard.HorizontalDimension.Categories.Count; i++)
            {
                for (int j = 0; j < kanbanBoard.VerticalDimension.Categories.Count; j++)
                {
                    kanbanBoard.Cells[i, j].ItemsCount = 0;
                    kanbanBoard.Cells[i, j].View.ItemContainer.Children.Clear();
                }
            }
        }

        private void RemoveCells()
        {
            var toRemoveItems = kanbanBoard.KanbanGrid.Children
                .OfType<CellView>()
                .ToArray();

            foreach (var toRemove in toRemoveItems)
            {
                kanbanBoard.KanbanGrid.Children.Remove(toRemove);
            }
        }

        private const int HeaderCellCount = 1;

        private readonly IKanbanBoard kanbanBoard;

        private readonly DefaultElementsDispenser elementsDispenser;
    }
}