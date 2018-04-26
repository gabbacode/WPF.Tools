using System.Collections.Generic;
using System.Linq;
using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;

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

        public KanbanData RefreshData()
        {
            var issues = redmineRepository
                .GetIssues(Configuration?.ProjectId)
                .ToArray();

            var data = kanbanRepository.GetKanbanData(Configuration, issues);

            return data;
        }

        public IEnumerable<Project> LoadProjects()
        {
            var projects = redmineRepository.GetProjects();

            return projects;
        }

        private readonly IKanbanConfigurationRepository kanbanRepository;
        private readonly IRedmineRepository redmineRepository;
    }
}