using System.IO;
using System.Reactive;
using System.Windows.Forms;
using Kanban.Desktop.LocalBase.Models;
using Kanban.Desktop.LocalBase.Views;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class StartupViewModel : ViewModelBase, IViewModel
    {
        public ReactiveList<string> BaseList { get; set; }
        public ReactiveCommand NewDbCommand { get; set; }
        public ReactiveCommand OpenDbCommand { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly IShell shell_;
        private readonly IAppModel appModel_;

        public StartupViewModel(IShell shell, IAppModel appModel)
        {
            shell_ = shell;
            appModel_ = appModel;

            appModel_.LoadConfig();

            var recent = appModel_.GetRecentDocuments();
            BaseList = new ReactiveList<string>(recent);
            
            OpenRecentDbCommand = ReactiveCommand.Create<string>(uri =>
            {
                if (File.Exists(uri))
                {
                    var scope = appModel_.LoadScope(uri);

                    shell_.ShowView<BoardView>(options: new UiShowOptions
                    {
                        Title = uri
                    });
                }
                else
                {
                    appModel_.RemoveRecent(uri);
                    appModel_.SaveConfig();

                    BaseList.Remove(uri);

                    DialogCoordinator.Instance.ShowMessageAsync(this, "Ошибка", "База была удалена или перемещена из данной папки");
                }
            });

            NewDbCommand = ReactiveCommand.Create(() =>
            {
                shell.ShowView<WizardView>();

                /*var basePath = this.model.CreateDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                this.model.ShowSelectedBaseTab(basePath);*/
            });

            OpenDbCommand = ReactiveCommand.Create(() =>
            {
                var openDialog = new OpenFileDialog()
                {
                    Filter = "SQLite DataBase | *.db",
                    Title = "Открытие базы"
                };

                if (openDialog.ShowDialog() == DialogResult.OK &&
                    !string.IsNullOrEmpty(openDialog.FileName))
                {
                    var scope = appModel_.LoadScope(openDialog.FileName);

                    shell_.ShowView<BoardView>(options: new UiShowOptions
                    {
                        Title = openDialog.FileName
                    });
                }
            });
        }//ctor
    }
}
