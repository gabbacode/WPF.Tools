using Data.Entities.Common.Redmine;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions.Generic;

namespace Kanban.Desktop.KanbanBoard
{
    public interface IKanbanBoardViewModel : IInitializibleViewModel, IViewModel
    {
        ObservableCollection<Issue> Issues { get; }

        TagDimension<string, Issue> VerticalDimension { get; }

        TagDimension<string, Issue> HorizontalDimension { get; }
        
        bool UseDynamicDimensionts { get; set; }
    }

    public class KanbanBoardViewModel : ReactiveObject, IKanbanBoardViewModel
    {
        public KanbanBoardViewModel(IKanbanConfigurationRepository kanbanRepository)
        {
            this.kanbanRepository = kanbanRepository;
            Title = "Kanban";            
        }

        public async void Initialize()
        {
            var data = await Task.Run(() => kanbanRepository.GetKanbanData());

            VerticalDimension = data.VerticalDimension;
            HorizontalDimension = data.HorizontalDimension;
            Issues = new ObservableCollection<Issue>(data.Issues);
        }
        
        [Reactive] public string Title { get; set; }
        
        public bool UseDynamicDimensionts { get; set; }

        [Reactive] public ObservableCollection<Issue> Issues { get; private set; }

        [Reactive] public TagDimension<string, Issue> VerticalDimension { get; private set; }

        [Reactive] public TagDimension<string, Issue> HorizontalDimension { get; private set; }

        private readonly IKanbanConfigurationRepository kanbanRepository;
    }
}
