using Autofac;
using Data.Sources.Common;
using Kanban.Desktop.KanbanBoard;
using Kanban.Desktop.Login;
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

            var shell = UiStarter.Start<IDockWindow>(new Bootstrap());

            AuthProcess.Start(
                getAutenticationData:      LoginDialog.GetAutenticationDataTask(),
                autentification:             (x) =>
                    {
                        if (x == null)
                            return false;

                        var authContext = shell.Container.Resolve<IAutentificationContext>();
                        return authContext.Login(x.Username, x.Password);
                    },
                autenticationSuccess:      () => shell.ShowView<IKanbanBoardView>(),
                autenticationFail:         () => Current.MainWindow.Close());
        }
    }
}
