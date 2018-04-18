using System.Collections.ObjectModel;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop
{
    internal class MainWindowViewModel
    {
        public MainWindowViewModel()
        {
            VerticalDimension = new TagDimension<string, Ticket>
                (
                    tags: new []{"A", "B", "C" },
                    getItemTags: (e) => new[] { e.Status },
                    categories: new IDimensionCategory[]
                    {
                        new TagsDimensionCategory<string>("A"),
                        new TagsDimensionCategory<string>("B"),
                        new TagsDimensionCategory<string>("C"),
                    }
                );

            HorizontalDimension = new TagDimension<string, Ticket>
                (
                    tags: new[] { "A", "B", "C" },
                    getItemTags: (e) => new[] { e.State },
                    categories: new IDimensionCategory[]
                    {
                        new TagsDimensionCategory<string>("A"),
                        new TagsDimensionCategory<string>("B"),
                        new TagsDimensionCategory<string>("C"),
                    }
                );


            Tickets = new ObservableCollection<Ticket>()
            {
                new Ticket(status: "A", state: "A"),
                new Ticket(status: "A", state: "A"),
                new Ticket(status: "A", state: "A"),
                new Ticket(status: "A", state: "A"),
                new Ticket(status: "A", state: "A"),
                new Ticket(status: "A", state: "B"),
                new Ticket(status: "A", state: "C"),
                new Ticket(status: "B", state: "A"),
                new Ticket(status: "B", state: "B"),
                new Ticket(status: "B", state: "C"),
                new Ticket(status: "C", state: "A"),
                new Ticket(status: "C", state: "B"),
                new Ticket(status: "C", state: "C"),
            };
        }

        public ObservableCollection<Ticket> Tickets { get; }

        public TagDimension<string, Ticket> VerticalDimension { get; }

        public ObservableCollection<IDimensionCategory> VerticalCategories { get; }

        public TagDimension<string, Ticket> HorizontalDimension { get; }

        public ObservableCollection<IDimensionCategory> HorizontalCategories { get; }
    }
}