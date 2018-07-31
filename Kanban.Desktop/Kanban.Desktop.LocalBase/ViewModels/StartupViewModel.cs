using System.IO;
using System.Linq;
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
        public ReactiveCommand<string,Unit> RemoveRecentCommand { get; set; }

        private readonly IShell shell;
        private readonly IAppModel appModel;

        public StartupViewModel(IShell shell, IAppModel appModel)
        {
            this.shell = shell;
            this.appModel = appModel;

            this.appModel.LoadConfig();

            var recent = this.appModel.GetRecentDocuments();
            BaseList = new ReactiveList<string>(recent);

            OpenRecentDbCommand = ReactiveCommand.Create<string>(uri =>
            {
                if (!OpenBoardView(uri))
                {
                    RemoveRecent(uri);
                    DialogCoordinator.Instance.ShowMessageAsync(this, "Ошибка", "База была удалена или перемещена из данной папки");
                }
            });

            RemoveRecentCommand = ReactiveCommand.Create<string>(RemoveRecent);

            NewDbCommand = ReactiveCommand.Create(() => shell.ShowView<WizardView>());

            OpenDbCommand = ReactiveCommand.Create(() =>
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "SQLite DataBase | *.db",
                    Title = "Открытие базы"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var uri = dialog.FileName;
                    OpenBoardView(uri);
                    AddRecent(uri);
                }
            });
        }//ctor

        private void RemoveRecent(string uri)
        {
            appModel.RemoveRecent(uri);
            appModel.SaveConfig();
            BaseList.Remove(uri);
        }

        private void AddRecent(string uri)
        {
            appModel.AddRecent(uri);
            appModel.SaveConfig();
            BaseList.Remove(uri);
            BaseList.Insert(0, uri);
        }

        private bool OpenBoardView(string uri)
        {
            var file = new FileInfo(uri);

            if (!file.Exists)
                return false;

            var scope = appModel.LoadScope(uri);

            shell.ShowView<BoardView>(
                viewRequest: new BoardViewRequest { Scope = scope },
                options: new UiShowOptions { Title = file.Name });

            AddRecent(uri);

            return true;
        }
    }
}
