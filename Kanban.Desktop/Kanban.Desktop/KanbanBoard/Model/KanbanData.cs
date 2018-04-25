using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanData
    {
        public int ProjectId { get; set; }

        public IDimension VerticalDimension { get; internal set; }

        public IDimension HorizontalDimension { get; internal set; }

        public IEnumerable<Issue> Issues { get; internal set; }

        public ICardContent CardElements { get; internal set; }
    }
}
