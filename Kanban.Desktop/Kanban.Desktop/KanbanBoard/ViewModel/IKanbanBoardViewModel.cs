using Data.Entities.Common.Redmine;
using System.Collections.ObjectModel;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{
    public interface IKanbanBoardViewModel : IInitializableViewModel, IViewModel
    {
        ObservableCollection<Issue> Issues { get; }

        IDimension VerticalDimension { get; }

        IDimension HorizontalDimension { get; }
        
        string ConfigutaionName { get; set; }
    }
}
