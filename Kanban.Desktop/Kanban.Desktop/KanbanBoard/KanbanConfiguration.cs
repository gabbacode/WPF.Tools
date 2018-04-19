using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop
{
    public class KanbanConfiguration
    {
        public TagDimension<string, Issue> VerticalDimension { get; internal set; }
        public TagDimension<string, Issue> HorizontalDimension { get; internal set; }
        public IEnumerable<Issue> Issues { get; internal set; }
    }
}
