using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using System.Linq;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;

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
            var issues = RedmineRepository.GetIssues()
                .ToArray();

            var users = issues
                .Where(i => i.AssignedTo != null)
                .Select(i => i.AssignedTo.Name)
                .OrderBy(n => n)
                .Distinct()
                .ToArray();

            var statuses = issues
                .Where(i => i.Status != null)
                .OrderBy(i => i.Status.Id)
                .Select(i => i.Status.Name)
                .Distinct()
                .ToArray();

            var configuration = new KanbanConfiguration
            {
                VerticalDimension = new TagDimension<string, Issue>
                (
                    tags: users,
                    getItemTags: (e) => new[] { e.AssignedTo?.Name },
                    categories : users
                        .Select(u => new TagsDimensionCategory<string>(u, u))
                        .Select(c => (IDimensionCategory) c)
                        .ToArray()
                ),

                HorizontalDimension = new TagDimension<string, Issue>
                (
                    tags: statuses,
                    getItemTags: (e) => new[] { e.Status.Name },
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
