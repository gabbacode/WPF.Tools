using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanData
    {
        public IDimension VerticalDimension { get; internal set; }

        public IDimension HorizontalDimension { get; internal set; }

        public IEnumerable<Issue> Issues { get; internal set; }

        public ICardItems CardElements { get; internal set; }
    }
}
