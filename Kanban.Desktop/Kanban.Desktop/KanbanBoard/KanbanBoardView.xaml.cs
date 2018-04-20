namespace Kanban.Desktop.KanbanBoard
{
    /// <summary>
    /// codebehind for KanbanBoardView.xaml
    /// </summary>
    public partial class KanbanBoardView : IKanbanBoardView
    {
        public KanbanBoardView(IKanbanBoardViewModel kanbanBoardViewModel)
        {
            InitializeComponent();
            kanbanBoardViewModel.Initialize();

            DataContext = kanbanBoardViewModel;
        }
    }
}
