using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using Kanban.Desktop.KanbanBoard.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{

    public class KanbanBoardViewModel : ReactiveObject, IKanbanBoardViewModel
    {
        public KanbanBoardViewModel(
            IKanbanConfigurationRepository kanbanRepository,
            IRedmineRepository redmineRepository)
        {
            KanbanRepository = kanbanRepository;
            RedmineRepository = redmineRepository;
            Title = "Kanban";

            Projects = new ReactiveList<Project>();
            Issues = new ReactiveList<Issue>();

            this.ObservableForProperty(x => x.CurrentProject)
                .Subscribe(x =>
                {
                    if (CurrentConfiguration != null)
                    {
                        CurrentConfiguration.ProjectId = x.Value.Id;
                    }

                    Refresh();
                });

            this.ObservableForProperty(x => x.ConfigurtaionName)
                .Subscribe(x => 
                {
                    CurrentConfiguration = KanbanRepository.GetConfiguration(x.Value);
                });
        }

        private async void Refresh()
        {
            var issues = await Task.Run(() => RedmineRepository.GetIssues(CurrentConfiguration?.ProjectId));

            var data = KanbanRepository.GetKanbanData(CurrentConfiguration, issues);

            VerticalDimension = data.VerticalDimension;
            HorizontalDimension = data.HorizontalDimension;
            CardContent = data.CardElements;
            CardsColors = data.CardsColors;

            Issues.Clear();
            Issues.AddRange(data.Issues);
        }

        public async void Initialize()
        {
            var projects = await Task.Run(() => RedmineRepository.GetProjects());

            Projects.Clear();
            foreach (var project in projects)
            {
                Projects.Add(project);
            }

            if (CurrentConfiguration != null
                && CurrentConfiguration.ProjectId.HasValue)
            {
                CurrentProject = Projects.FirstOrDefault(x => x.Id == CurrentConfiguration.ProjectId);
            }

            Refresh();
        }
        
        [Reactive] public string Title { get; set; }

        [Reactive] public string ConfigurtaionName { get; set; }

        [Reactive] public Project CurrentProject { get; set; }

        [Reactive] public ReactiveList<Project> Projects { get; set; }

        [Reactive] public ReactiveList<Issue> Issues { get; private set; }

        [Reactive] public IDimension VerticalDimension { get; private set; }

        [Reactive] public IDimension HorizontalDimension { get; private set; }

        [Reactive] public ICardContent CardContent { get; private set; }

        [Reactive] public ICardsColors CardsColors { get; private set; }

        public KanbanConfiguration CurrentConfiguration { get; private set; }

        private readonly IKanbanConfigurationRepository KanbanRepository;

        private readonly IRedmineRepository RedmineRepository;
    }
}
