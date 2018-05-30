using Autofac;
using Data.Sources.Common;
using Kanban.Desktop.KanbanBoard;
using System.Windows;
using Kanban.Desktop.Settings;
using Ui.Wpf.Common;
using Kanban.Desktop.KanbanBoard.View;
using Kanban.Desktop.Issues;
using Kanban.Desktop.Issues.View;

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
                getAutenticationData: () => LoginDialog.GetAutenticationDataTask(),
                autentification: (x) =>
                    {
                        if (x == null)
                            return false;

                        var authContext = shell.Container.Resolve<IAuthenticationContext>();
                        return authContext.Login(x.Username, x.Password);
                    },
                autenticationSuccess: () =>
                    {
                        shell.ShowView<ISettingsView>(
                            options: new UiShowOptions
                            {
                                Title = "Settings"                            
                            });
                        shell.ShowView<IKanbanBoardView>();
                        shell.ShowView<IKanbanBoardView>(
                            new KanbanViewRequest
                            {
                                ConfigurtaionName = "h_status_v_assigned_c_subject_treker_details"
                            },
                            new KanbanShowOptions
                            {
                                Title = "Kanban dynamic dimension",
                            });
                        shell.ShowView<IKanbanBoardView>(
                            new KanbanViewRequest
                            {
                                ConfigurtaionName = "ods"
                            },
                            new KanbanShowOptions
                            {
                                Title = "ods",                                
                            });

                        shell.ShowView<IIssueView>(
                            new IssueViewRequest
                            {
                                IssueId = 4404
                            });

                        shell.ShowTool<IIssuesTool>();
                    },
                autenticationFail: () => Current.MainWindow.Close());
        }
    }
}
