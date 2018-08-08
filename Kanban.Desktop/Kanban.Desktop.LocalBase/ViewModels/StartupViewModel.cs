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
        public ReactiveCommand NewFileCommand { get; set; }
        public ReactiveCommand NewBoardCommand { get; set; }
        public ReactiveCommand OpenFileCommand { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }
        public ReactiveCommand<string, Unit> RemoveRecentCommand { get; set; }

        private readonly IShell shell;
        private readonly IAppModel appModel;

        public StartupViewModel(IShell shell, IAppModel appModel)
        {
            this.shell = shell;
            this.appModel = appModel;

            this.appModel.LoadConfig();

            var recent = this.appModel.GetRecentDocuments();
            BaseList = new ReactiveList<string>(recent.Take(3));

            OpenRecentDbCommand = ReactiveCommand.Create<string>(uri =>
            {
                if (!OpenBoardView(uri))
                {
                    RemoveRecent(uri);
                    DialogCoordinator.Instance.ShowMessageAsync(this, "Ошибка",
                        "Файл был удалён или перемещён из данной папки");
                }
            });

            RemoveRecentCommand = ReactiveCommand.Create<string>(RemoveRecent);

            NewFileCommand = ReactiveCommand.Create(() => shell.ShowView<WizardView>());

            NewBoardCommand = ReactiveCommand.Create(() =>
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "SQLite DataBase | *.db",
                    Title = "Выбор существующего файла базы данных"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var uri = dialog.FileName;
                    AddRecent(uri);
                    shell.ShowView<WizardView>(new WizardViewRequest() { Step = 2 });
                }
            });

            OpenFileCommand = ReactiveCommand.Create(() =>
            {
                var dialog = new OpenFileDialog()
                {
                    Filter = "SQLite DataBase | *.db",
                    Title = "Выбор существующего файла базы данных"
                };

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    var uri = dialog.FileName;
                    OpenBoardView(uri);
                    AddRecent(uri);
                }
            });
        } //ctor

        private void RemoveRecent(string uri)
        {
            appModel.RemoveRecent(uri);
            appModel.SaveConfig();
            BaseList.PublishCollection(appModel.GetRecentDocuments().Take(3));
        }

        private void AddRecent(string uri)
        {
            appModel.AddRecent(uri);
            appModel.SaveConfig();
            BaseList.Remove(uri);
            BaseList.Insert(0, uri);
            if (BaseList.Count > 3) BaseList.RemoveAt(3);
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
