using Data.Entities.Common.Redmine;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class FiltersData
    {
        public Project[] Projects { get; internal set; }

        public Priority[] Priorities { get; internal set; }
    }
}