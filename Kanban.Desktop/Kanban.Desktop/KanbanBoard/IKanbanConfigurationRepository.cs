using Kanban.Desktop.KanbanBoard.Model;

namespace Kanban.Desktop.KanbanBoard
{
    public interface IKanbanConfigurationRepository
    {
        KanbanData GetKanbanData(string configurationName);
    }
}
