using Data.Entities.Common.Redmine;
using ReactiveUI;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{
    public interface IKanbanBoardViewModel : IInitializableViewModel, IViewModel
    {
        ReactiveList<Issue> Issues { get; }

        IDimension VerticalDimension { get; }

        IDimension HorizontalDimension { get; }
        
        string ConfigurtaionName { get; set; }
    }
}
