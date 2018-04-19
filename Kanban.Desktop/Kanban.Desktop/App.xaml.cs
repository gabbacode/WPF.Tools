using Kanban.Desktop.KanbanBoard;
using System.Windows;
using Ui.Wpf.Common;

namespace Kanban.Desktop
{
    /// <summary>
    /// App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            UiStarter.Start<IDockWindow, IKanbanBoardView>(
                new Bootstrap());
        }
    }
}
