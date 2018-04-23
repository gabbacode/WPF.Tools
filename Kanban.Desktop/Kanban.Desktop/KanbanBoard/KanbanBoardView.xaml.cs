using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.KanbanBoard
{
    /// <summary>
    /// codebehind for KanbanBoardView.xaml
    /// </summary>
    public partial class KanbanBoardView : IKanbanBoardView, IInitializibleViewModel
    {
        public KanbanBoardView(IKanbanBoardViewModel kanbanBoardViewModel)
        {
            InitializeComponent();

            ViewModel = kanbanBoardViewModel;
        }

        public IViewModel ViewModel { get; set; }
        
        public void Configure(UiShowOptions options)
        {
            if (ViewModel is IKanbanBoardViewModel kanbanBoardViewModel
                && options is KanbanShowOptions kanbanOptions )
            {
                kanbanBoardViewModel.UseDynamicDimensionts = kanbanOptions.UseDynamicDimensionts;
            }            
            
            ViewModel.Title = options.Title;
            
        }

        public void Initialize()
        {
            (ViewModel as IKanbanBoardViewModel)?.Initialize();
            DataContext = ViewModel;
        }
    }
}
