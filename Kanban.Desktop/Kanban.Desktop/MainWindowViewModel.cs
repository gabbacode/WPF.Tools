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
                new DimensionCategory(),
                new DimensionCategory(),
            };

            HorizontalCategories = new ObservableCollection<IDimensionCategory>()
            {
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
                new DimensionCategory(),
            };

            Tickets = new ObservableCollection<Ticket>()
            {
                new Ticket(),
                new Ticket(),
                new Ticket(),
                new Ticket(),
                new Ticket(),
                new Ticket(),
                new Ticket(),
                new Ticket(),
            };
        }

        public ObservableCollection<Ticket> Tickets { get; }

        public ObservableCollection<IDimensionCategory> VerticalCategories { get; }

        public ObservableCollection<IDimensionCategory> HorizontalCategories { get; }
    }
}