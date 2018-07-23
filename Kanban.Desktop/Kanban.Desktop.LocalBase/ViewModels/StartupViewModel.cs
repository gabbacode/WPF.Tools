using System.IO;
using System.Reactive;
using Kanban.Desktop.LocalBase.Models;
using Kanban.Desktop.LocalBase.Views;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class StartupViewModel : ViewModelBase, IViewModel
    {
        public ReactiveList<string> BaseList { get; set; }
        public ReactiveCommand NewDbCommand { get; set; }
        public ReactiveCommand OpenDbCommand { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly StartupModel model;
        private readonly IShell shell_;
        private readonly IAppModel appModel_;

        public StartupViewModel(StartupModel model, IShell shell, IAppModel appModel)
        {
            this.model = model;
            shell_ = shell;
            appModel_ = appModel;

            appModel_.Load();

            var recent = appModel_.GetRecentDocuments();
            BaseList = new ReactiveList<string>(recent);
            
            OpenRecentDbCommand = ReactiveCommand.Create<string>(basePath =>
            {
                if (File.Exists(basePath))
                    this.model.ShowSelectedBaseTab(basePath);
                else
                {
                    appModel_.RemoveRecent(basePath);
                    appModel_.Save();

                    BaseList.Clear();
                    BaseList.AddRange(recent);

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
                var basePath = this.model.OpenDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                this.model.ShowSelectedBaseTab(basePath);
            });
        }
    }
}
