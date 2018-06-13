using Data.Entities.Common.Redmine;
using Data.Entities.Common.IssuesBoard;
using System;
using System.Collections.Generic;
using System.Linq;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanConfigurationRepository : IKanbanConfigurationRepository
    {
        public KanbanConfiguration GetConfiguration(string configurationName)
        {
            if (string.IsNullOrEmpty(configurationName))
                return null;

            return KanbanConfiguration.Parse((string)Properties.Settings.Default[configurationName]);
        }

        public KanbanData GetKanbanData(KanbanConfiguration configuration, ICollection<Issue> issues)
        {
            if (configuration == null)
                return GetDefaultConfiguration(issues);

            return GetStoredConfiguration(configuration, issues);

        }
        
        private KanbanData GetStoredConfiguration(KanbanConfiguration config, IEnumerable<Issue> issues)
        {
            return new KanbanData
            {
                HorizontalDimension = new TagDimension(
                    config.HorizontalDimension.DimensionName, 
                    config.HorizontalDimension.SortingPath,
                    config.HorizontalDimension.ValuesFilter),

                VerticalDimension = new TagDimension(
                    config.VerticalDimension.DimensionName, 
                    config.VerticalDimension.ValuesFilter),

                CardElements = new CardContent(
                    config.CardItemsConfiguration.CardsItems
                        .Select(ci =>
                        {
                            Enum.TryParse(ci.Area, out CardContentArea cardContentArea);

                            return new CardContentItem(
                                ci.Path,
                                cardContentArea);
                        })
                        .OfType<ICardContentItem>()
                        .ToArray()),
                CardsColors = new CardsColors
                {
                    Path = config.CardItemsConfiguration.ColorSettings.Path,
                    ColorMap = config.CardItemsConfiguration.ColorSettings.ColorMaps
                        .ToDictionary(
                            k => (object)k.Value,
                            v => (ICardColor)new CardColor
                            {
                                Background = v.CardColors.BackgroundColor,
                                BorderBrush = v.CardColors.BorderColor
                            })
                },
                Issues = issues
            };
        }

        private KanbanData GetDefaultConfiguration(ICollection<Issue> issues)
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

                CardElements = new CardContent(new ICardContentItem[] 
                {
                    new CardContentItem("Subject"),
                    new CardContentItem("Tracker"),
                    new CardContentItem("Priority"),
                    new CardContentItem("Description")
                }),

                Issues = issues
            };
            return configuration;
        }
    }
}
