using Data.Entities.Common.Redmine;
using ReactiveUI;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{
    public interface IKanbanBoardViewModel : IInitializableViewModel, IViewModel
    {
        Project CurrentProject { get; set; }

        ReactiveList<Project> Projects { get; }
        
        ReactiveList<Issue> Issues { get; }

        IDimension VerticalDimension { get; }

        IDimension HorizontalDimension { get; }
        
        ICardContent CardContent { get; }

        ICardsColors CardsColors { get; }
        
        
        string ConfigurtaionName { get; set; }
    }
}
