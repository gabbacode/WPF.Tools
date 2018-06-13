using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Data.Entities.Common.IssuesBoard;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public interface IKanbanConfigurationRepository
    {
        KanbanConfiguration GetConfiguration(string configurationName);

        KanbanData GetKanbanData(KanbanConfiguration configuration, ICollection<Issue> issues);
    }
}
