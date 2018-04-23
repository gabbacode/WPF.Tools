using Data.Entities.Common.Redmine;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop.KanbanBoard.Model
{
    public class KanbanBoardModel : ReactiveObject, IKanbanBoardModel
    {
        public KanbanBoardModel(IKanbanConfigurationRepository kanbanRepository)
        {
            KanbanRepository = kanbanRepository;
        }

        [Reactive] public ObservableCollection<Issue> Issues { get; private set; }



        [Reactive] public IDimension VerticalDimension { get; private set; }

        [Reactive] public IDimension HorizontalDimension { get; private set; }

        private readonly IKanbanConfigurationRepository KanbanRepository;
    }
}
