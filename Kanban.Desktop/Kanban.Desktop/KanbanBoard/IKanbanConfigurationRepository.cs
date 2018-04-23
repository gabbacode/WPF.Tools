using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Kanban.Desktop.KanbanBoard.Model;

namespace Kanban.Desktop.KanbanBoard
{
    public interface IKanbanConfigurationRepository
    {
        KanbanConfiguration GetConfiguration(string configurationName);

        KanbanData GetKanbanData(KanbanConfiguration configuration, IEnumerable<Issue> issues);
    }
}
