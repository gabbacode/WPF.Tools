using Data.Entities.Common.Redmine;
using Kanban.Desktop.KanbanBoard.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{

    public class KanbanBoardViewModel : ReactiveObject, IKanbanBoardViewModel
    {
        public KanbanBoardViewModel(
            IKanbanBoardModel model)
        {
            this.model = model;
            
            Title = "Kanban";

            Projects = new ReactiveList<Project>();
            Issues = new ReactiveList<Issue>();

            this.ObservableForProperty(x => x.CurrentProject)
                .Subscribe(x =>
                {
                    if (model.Configuration != null)
                    {
                        model.Configuration.ProjectId = x.Value.Id;
                    }

                    Refresh();
                });

            this.ObservableForProperty(x => x.ConfigurtaionName)
                .Subscribe(x => 
                {
                    model.GetConfiguration(x.Value);
                });
        }

        private async void Refresh()
        {
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);
            var data = await Task.Run(() => model.RefreshData()).ConfigureAwait(true);
            Debug.WriteLine(Thread.CurrentThread.ManagedThreadId);            
            Issues.Clear();
            VerticalDimension = data.VerticalDimension;
            HorizontalDimension = data.HorizontalDimension;
            CardContent = data.CardElements;
            CardsColors = data.CardsColors;
            Issues.AddRange(data.Issues);
        }

        public async void Initialize()
        {
            var projects = await Task.Run(() => model.LoadProjects()).ConfigureAwait(true);

            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }

            if (model.Configuration != null
                && model.Configuration.ProjectId.HasValue)
            {
                CurrentProject = Projects.FirstOrDefault(x => x.Id == model.Configuration.ProjectId);
            }

            Refresh();
        }
        
        [Reactive] public string Title { get; set; }

        [Reactive] public string ConfigurtaionName { get; set; }

        [Reactive] public Project CurrentProject { get; set; }

        [Reactive] public ReactiveList<Project> Projects { get; private set; }

        [Reactive] public ReactiveList<Issue> Issues { get; private set; }

        [Reactive] public IDimension VerticalDimension { get; private set; }

        [Reactive] public IDimension HorizontalDimension { get; private set; }

        [Reactive] public ICardContent CardContent { get; private set; }

        [Reactive] public ICardsColors CardsColors { get; private set; }

        private readonly IKanbanBoardModel model;
    }
}
