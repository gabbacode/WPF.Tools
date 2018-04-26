using Autofac;
using Data.Sources.Common;
using Kanban.Desktop.KanbanBoard;
using Kanban.Desktop.Login;
using System.Windows;
using Kanban.Desktop.Settings;
using Ui.Wpf.Common;
using Kanban.Desktop.KanbanBoard.View;

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

            var shell = UiStarter.Start<IDockWindow>(new Bootstrap());
            shell.Title = "Kanban.Desktop";

            AuthProcess.Start(
                getAutenticationData:      LoginDialog.GetAutenticationDataTask(),
                autentification:             (x) =>
                    {
                        if (x == null)
                            return false;

                        var authContext = shell.Container.Resolve<IAutentificationContext>();
                        return authContext.Login(x.Username, x.Password);
                    },
                autenticationSuccess:      () =>
                    {
                        shell.ShowView<ISettingsView>(new UiShowOptions
                        {
                            Title = "Settings"                            
                        });
                        shell.ShowView<IKanbanBoardView>();
                        shell.ShowView<IKanbanBoardView>(new KanbanShowOptions
                        {
                            Title = "Kanban dynamic dimension",
                            ConfigurtaionName = "h_status_v_assigned_c_subject_treker_details"
                        });
                        shell.ShowView<IKanbanBoardView>(new KanbanShowOptions
                        {
                            Title = "ods",
                            ConfigurtaionName = "ods"
                        });
                    },
                autenticationFail:         () => Current.MainWindow.Close());
        }
    }
}
