using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using System.Linq;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop.KanbanBoard
{
    public class KanbanConfigurationRepository : IKanbanConfigurationRepository
    {
        public KanbanConfigurationRepository(IRedmineRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public IRedmineRepository RedmineRepository { get; }

        public KanbanConfiguration GetKanbanData()
        {
            var issues = RedmineRepository.GetIssues();
            var statuses = RedmineRepository.GetStatuses();
            var users = RedmineRepository.GetUsers();

            var configuration = new KanbanConfiguration
            {
                VerticalDimension = new TagDimension<string, Issue>
                (
                    tags: statuses.ToArray(),
                    getItemTags: (e) => new[] { e.AssignedTo },
                    categories : users
                        .Select(s => new TagsDimensionCategory<string>(s, s))
                        .Select(s => (IDimensionCategory) s)
                        .ToArray()
                ),

                HorizontalDimension = new TagDimension<string, Issue>
                (
                    tags: statuses.ToArray(),
                    getItemTags: (e) => new[] { e.Status },
                    categories: statuses
                        .Select(s => new TagsDimensionCategory<string>(s, s))
                        .Select(s => (IDimensionCategory)s)
                        .ToArray()
                ),

                Issues = issues
            };

            return configuration;
        }
    }
}
