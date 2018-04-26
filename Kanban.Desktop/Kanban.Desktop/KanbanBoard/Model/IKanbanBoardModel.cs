using System.Collections.Generic;
using Data.Entities.Common.Redmine;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public interface IKanbanBoardModel
    {
        KanbanConfiguration Configuration { get; set; }
        
        void GetConfiguration(string configurationName);
        
        KanbanData RefreshData();
        
        IEnumerable<Project> LoadProjects();
    }
}