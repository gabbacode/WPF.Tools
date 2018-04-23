using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using Kanban.Desktop.KanbanBoard.Model;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;
using Ui.Wpf.KanbanControl.Elements;

namespace Kanban.Desktop.KanbanBoard
{
    public class KanbanConfigurationRepository : IKanbanConfigurationRepository
    {
        public KanbanConfigurationRepository(IRedmineRepository redmineRepository)
        {
            RedmineRepository = redmineRepository;
        }

        public IRedmineRepository RedmineRepository { get; }

        public KanbanData GetKanbanData(string configurationName)
        {
            if (string.IsNullOrEmpty(configurationName))
                return GetDefaultConfiguration();

            return GetStoredConfiguration(configurationName);

        }

        private KanbanData GetStoredConfiguration(string configurationName)
        {
            var config = KanbanConfiguration.Parse((string)Properties.Settings.Default[configurationName]);

            var issues = RedmineRepository.GetIssues()
                .ToArray();

            return new KanbanData
            {
                HorizontalDimension = new TagDimension(config.HorizontalDimension.DimensionName, config.HorizontalDimension.ValuesFilter),
                VerticalDimension = new TagDimension(config.VerticalDimension.DimensionName, config.VerticalDimension.ValuesFilter),
                CardElements = new CardItems(config.CardItems),
                Issues = issues
            };
        }

        private KanbanData GetDefaultConfiguration()
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

                Issues = issues
            };
            return configuration;
        }
    }
}
