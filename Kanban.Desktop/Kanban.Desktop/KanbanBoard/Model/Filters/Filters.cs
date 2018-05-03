using System;
using Data.Entities.Common.Redmine;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class Filters
    {
        public Project[] Projects { get; internal set; }

        public Priority[] Priorities { get; internal set; }

        public DateTime? DateFrom { get; internal set; }

        public DateTime? DateTo { get; internal set; }
    }
}
