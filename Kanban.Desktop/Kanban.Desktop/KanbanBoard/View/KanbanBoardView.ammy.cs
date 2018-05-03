using Kanban.Desktop.KanbanBoard.ViewModel;
using System.Windows.Controls;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.KanbanBoard.View
{
    /// <summary>
    /// codebehind for KanbanBoardView.xaml
    /// </summary>
    public partial class KanbanBoardView : IKanbanBoardView
    {
        public KanbanBoardView(IKanbanBoardViewModel kanbanBoardViewModel)
        {
            InitializeComponent();
            ViewModel = kanbanBoardViewModel;
            DataContext = ViewModel;
        }

        public IViewModel ViewModel { get; set; }
        
        public void Configure(UiShowOptions options)
        {
            if (ViewModel is IKanbanBoardViewModel kanbanBoardViewModel
                && options is KanbanShowOptions kanbanOptions )
            {
                kanbanBoardViewModel.ConfigurtaionName = kanbanOptions.ConfigurtaionName;
            }            
            
            ViewModel.Title = options.Title;
            
        }
    }
}
