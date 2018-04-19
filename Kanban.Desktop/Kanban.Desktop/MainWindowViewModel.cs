using System.Collections.ObjectModel;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop
{
    interface IMainWindowViewModel
    {
        ObservableCollection<Ticket> Tickets { get; }
        
        TagDimension<string, Ticket> VerticalDimension { get; }

        TagDimension<string, Ticket> HorizontalDimension { get; }
    }
    
    internal class MainWindowViewModel : IMainWindowViewModel
    {
        public MainWindowViewModel()
        {
            VerticalDimension = new TagDimension<string, Ticket>
                (
                    tags: new[] { "X", "Y", "O" },
                    getItemTags: (e) => new[] { e.Status },
                    categories: new IDimensionCategory[]
                    {
                        new TagsDimensionCategory<string>("Chi", "X"),
                        new TagsDimensionCategory<string>("Psi", "Y"),
                        new TagsDimensionCategory<string>("Omega", "O"),
                    }
                );

            HorizontalDimension = new TagDimension<string, Ticket>
                (
                    tags: new []{"A", "B", "C" },
                    getItemTags: (e) => new[] { e.State },
                    categories: new IDimensionCategory[]
                    {
                        new TagsDimensionCategory<string>("Alpha", "A"),
                        new TagsDimensionCategory<string>("Bravo", "B"),
                        new TagsDimensionCategory<string>("Charlie", "C"),
                    }
                );


            Tickets = new ObservableCollection<Ticket>()
            {
                new Ticket(status: "X", state: "A"),
                new Ticket(status: "X", state: "A"),
                new Ticket(status: "X", state: "A"),
                new Ticket(status: "X", state: "A"),
                new Ticket(status: "X", state: "A"),
                new Ticket(status: "X", state: "B"),
                new Ticket(status: "X", state: "C"),
                new Ticket(status: "Y", state: "A"),
                new Ticket(status: "Y", state: "B"),
                new Ticket(status: "Y", state: "C"),
                new Ticket(status: "O", state: "A"),
                new Ticket(status: "O", state: "B"),
                new Ticket(status: "O", state: "C"),
            };
        }

        public ObservableCollection<Ticket> Tickets { get; }

        public TagDimension<string, Ticket> VerticalDimension { get; }

        public TagDimension<string, Ticket> HorizontalDimension { get; }

    }
}