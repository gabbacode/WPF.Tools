using Autofac;
using Data.Sources.Common;
using Kanban.Desktop.KanbanBoard;
using System.Windows;
using Kanban.Desktop.Settings;
using Ui.Wpf.Common;
using Kanban.Desktop.KanbanBoard.View;
using Kanban.Desktop.Issues;
using Kanban.Desktop.Issues.View;
using Ui.Wpf.Common.ShowOptions;

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

            var shell = UiStarter.Start<IDockWindow>(
                new Bootstrap(), 
                new UiShowStartWindowOptions
                {
                    Title = "Kanban.Desktop",
                    ToolPaneWidth = 100
                });

            AuthProcess.Start(
                getAuthenticationData: () => LoginDialog.GetAutenticationDataTask(),
                authentication: async (x) =>
                    {
                        if (x == null)
                            return false;

                        var authContext = shell.Container.Resolve<IAuthenticationContext>();
                        return await authContext.LoginAsync(x.Username, x.Password);
                    },
                authenticationSuccess: () =>
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
                                IssueId = 4689
                            });

                        shell.ShowTool<IIssuesTool>();
                    },
                authenticationFail: () => Current.MainWindow.Close());
        }
    }
}
