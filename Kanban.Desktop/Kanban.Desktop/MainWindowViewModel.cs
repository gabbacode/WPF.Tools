using System.Collections.ObjectModel;
using Ui.Wpf.KanbanControl;

namespace Kanban.Desktop
{
    internal class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            VerticalCategories = new ObservableCollection<IDimensionCategory>()
            {
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
            };

            HorizontalCategories = new ObservableCollection<IDimensionCategory>()
            {
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
            };

        }

        public ObservableCollection<IDimensionCategory> VerticalCategories { get; }

        public ObservableCollection<IDimensionCategory> HorizontalCategories { get; }
    }
}