using Data.Entities.Common.Redmine;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;

namespace Kanban.Desktop.KanbanBoard.ViewModel
{

    public class KanbanBoardViewModel : ReactiveObject, IKanbanBoardViewModel
    {
        public KanbanBoardViewModel(IKanbanConfigurationRepository kanbanRepository)
        {
            this.kanbanRepository = kanbanRepository;
            Title = "Kanban";            
        }

        public async void Initialize()
        {
            var data = await Task.Run(() => kanbanRepository.GetKanbanData(ConfigutaionName));

            VerticalDimension = data.VerticalDimension;
            HorizontalDimension = data.HorizontalDimension;
            Issues = new ObservableCollection<Issue>(data.Issues);
        }
        
        [Reactive] public string Title { get; set; }
        
        public string ConfigutaionName { get; set; }

        [Reactive] public ObservableCollection<Issue> Issues { get; private set; }

        [Reactive] public IDimension VerticalDimension { get; private set; }

        [Reactive] public IDimension HorizontalDimension { get; private set; }

        private readonly IKanbanConfigurationRepository kanbanRepository;
    }
}
