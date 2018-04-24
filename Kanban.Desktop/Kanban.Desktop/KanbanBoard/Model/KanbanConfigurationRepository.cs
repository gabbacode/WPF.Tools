using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using System.Collections.Generic;
using System.Linq;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;
using Ui.Wpf.KanbanControl.Elements;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanConfigurationRepository : IKanbanConfigurationRepository
    {
        public KanbanConfigurationRepository(IRedmineRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public IRedmineRepository RedmineRepository { get; }

        public KanbanConfiguration GetConfiguration(string configurationName)
        {
            if (string.IsNullOrEmpty(configurationName))
                return null;

            return KanbanConfiguration.Parse((string)Properties.Settings.Default[configurationName]);
        }

        public KanbanData GetKanbanData(KanbanConfiguration configuration, IEnumerable<Issue> issues)
        {
            if (configuration == null)
                return GetDefaultConfiguration(issues);

            return GetStoredConfiguration(configuration, issues);

        }
        
        private KanbanData GetStoredConfiguration(KanbanConfiguration config, IEnumerable<Issue> issues)
        {
            return new KanbanData
            {
                HorizontalDimension = new TagDimension(config.HorizontalDimension.DimensionName, config.HorizontalDimension.ValuesFilter),
                VerticalDimension = new TagDimension(config.VerticalDimension.DimensionName, config.VerticalDimension.ValuesFilter),
                CardElements = new CardContent(config.CardsItemsConfiguration.CardsItemsPaths),
                Issues = issues
            };
        }

        private KanbanData GetDefaultConfiguration(IEnumerable<Issue> issues)
        {
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

            var configuration = new KanbanData
            {
                VerticalDimension = new TagDimension<string, Issue>
                (
                    tags: users,
                    getItemTags: (e) => new[] { e.AssignedTo?.Name },
                    categories: users
                        .Select(u => new TagsDimensionCategory<string>(u, u))
                        .Select(c => (IDimensionCategory)c)
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

                CardElements = new CardContent(new[] 
                {
                    "Subject", "Treker", "Priority", "Details"
                }),

                Issues = issues
            };
            return configuration;
        }
    }
}
