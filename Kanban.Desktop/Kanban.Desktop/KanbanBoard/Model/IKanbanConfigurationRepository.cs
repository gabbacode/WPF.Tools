using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Kanban.Desktop.KanbanBoard.Model.Configuration;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public interface IKanbanConfigurationRepository
    {
        KanbanConfiguration GetConfiguration(string configurationName);

        KanbanData GetKanbanData(KanbanConfiguration configuration, ICollection<Issue> issues);
    }
}
