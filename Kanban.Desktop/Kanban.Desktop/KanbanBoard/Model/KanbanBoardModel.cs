using System.Collections.Specialized;
using System.Linq;
using Data.Sources.Common.Redmine;
using Kanban.Desktop.KanbanBoard.Model.Configuration;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanBoardModel : IKanbanBoardModel
    {
        public KanbanBoardModel(
            IKanbanConfigurationRepository kanbanRepository,
            IRedmineRepository redmineRepository)
        {
            this.kanbanRepository = kanbanRepository;
            this.redmineRepository = redmineRepository;
        }

        public KanbanConfiguration Configuration { get; set; }
        
        public void GetConfiguration(string configurationName)
        {
            Configuration = kanbanRepository.GetConfiguration(configurationName);
        }

        public KanbanData LoadData(Filters filters)
        {
            var filterParameters = new NameValueCollection();
            if (filters.Projects != null)
            {
                foreach (var project in filters.Projects)
                {
                    filterParameters.Add("ProjectId", project.Id.ToString());
                }
            }
            if (filters.Priorities != null)
            {
                foreach (var priority in filters.Priorities)
                {
                    filterParameters.Add("PriorityId", priority.Id.ToString());
                }
            }

            var issues = redmineRepository
                .GetIssues(filterParameters)
                .Where(i => 
                    (filters.DateFrom == null || filters.DateFrom <= i.CreatedOn)
                    && (filters.DateTo == null || filters.DateTo >= i.CreatedOn))
                .ToArray();

            var data = kanbanRepository.GetKanbanData(Configuration, issues);

            return data;
        }

        public FiltersData LoadFiltersData()
        {
            var data = new FiltersData();

            data.Projects = redmineRepository.GetProjects()
                .ToArray();

            data.Priorities = redmineRepository.GetPriorities()
                .ToArray();

            return data;
        }

        private readonly IKanbanConfigurationRepository kanbanRepository;
        private readonly IRedmineRepository redmineRepository;
    }
}