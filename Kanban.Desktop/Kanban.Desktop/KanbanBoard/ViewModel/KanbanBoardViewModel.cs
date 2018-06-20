using Data.Entities.Common.Redmine;
using Kanban.Desktop.Issues;
using Kanban.Desktop.Issues.View;
using Kanban.Desktop.KanbanBoard.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{

    public class KanbanBoardViewModel : ViewModelBase, IKanbanBoardViewModel
    {
        public KanbanBoardViewModel(
            IKanbanBoardModel model,
            IShell shell)
        {
            this.model = model;
            
            Title = "Kanban";

            Refresh = ReactiveCommand.CreateFromTask(DoRefresh);

            EditCardCommand = ReactiveCommand.Create<object>(o =>
            {
                var issue = o as Issue;
                shell.ShowView<IIssueView>(
                    new IssueViewRequest
                    {
                        IssueId = issue.Id
                    });
            });

        }

        private async Task DoRefresh()
        {
            using (var busyContext = GetBusyContext())
            {
                var filters = BuildFilters();

                var data = await Task
                    .Run(() => model.LoadData(filters))
                    .ConfigureAwait(true);

                Issues.Clear();
                VerticalDimension = data.VerticalDimension;
                HorizontalDimension = data.HorizontalDimension;
                CardContent = data.CardElements;
                CardsColors = data.CardsColors;
                Issues.AddRange(data.Issues);
            }
        }

        public async void Initialize(ViewRequest viewRequest)
        {
            model.GetConfiguration((viewRequest as KanbanViewRequest)?.ConfigurtaionName);

            var filtersData = await Task
                .Run(() => model.LoadFiltersData())
                .ConfigureAwait(true);

            Projects.Clear();
            foreach (var project in filtersData.Projects)
            {
                Projects.Add(project);
            }


            Priorities.Clear();
            foreach (var priority in filtersData.Priorities)
            {
                Priorities.Add(priority);
            }


            if (model.Configuration != null
                && model.Configuration.ProjectId.HasValue)
            {
                CurrentProject = Projects.FirstOrDefault(x => x.Id == model.Configuration.ProjectId);
            }

            await DoRefresh();
        }

        private Filters BuildFilters()
        {
            var filters = new Filters
            {
                Projects = CurrentProject != null
                    ? new[] { CurrentProject }
                    : null,

                Priorities = CurrentPriority != null
                    ? new[] { CurrentPriority }
                    : null,

                DateFrom = DateFrom,

                DateTo = DateTo
            };

            return filters;
        }

        private BusyContext GetBusyContext()
        {
            return new BusyContext(() => IsBusy = true, () => IsBusy = false);
        }

        [Reactive] public ICommand Refresh { get; private set; }

        [Reactive] public ICommand EditCardCommand { get; private set; }

        [Reactive] public string ConfigurtaionName { get; set; }

        [Reactive] public Project CurrentProject { get; set; }

        [Reactive] public ReactiveList<Project> Projects { get; private set; } = new ReactiveList<Project>();

        [Reactive] public Priority CurrentPriority { get; set; }

        [Reactive] public ReactiveList<Priority> Priorities { get; private set; } = new ReactiveList<Priority>();

        [Reactive] public DateTime? DateFrom { get; set; }

        [Reactive] public DateTime? DateTo { get; set; }

        [Reactive] public ReactiveList<Issue> Issues { get; private set; } = new ReactiveList<Issue>();

        [Reactive] public IDimension VerticalDimension { get; private set; }

        [Reactive] public IDimension HorizontalDimension { get; private set; }

        [Reactive] public ICardContent CardContent { get; private set; }

        [Reactive] public ICardsColors CardsColors { get; private set; }

        private readonly IKanbanBoardModel model;
    }
}
