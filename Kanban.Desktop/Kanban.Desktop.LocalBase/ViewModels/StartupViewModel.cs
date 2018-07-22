using System.Reactive;
using Kanban.Desktop.LocalBase.DataBaseSelector.Model;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.DataBaseSelector.ViewModel
{
    public class StartupViewModel : ViewModelBase, IViewModel
    {
        public ReactiveList<string> BaseList { get; set; }
        public ReactiveCommand NewDbCommand { get; set; }
        public ReactiveCommand OpenDbCommand { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly StartupModel model;

        public StartupViewModel(StartupModel model)
        {
            this.model = model;

            BaseList = new ReactiveList<string>();
            var list = this.model.GetBaseList();
            foreach (var addr in list) BaseList.Add(addr);

            OpenRecentDbCommand = ReactiveCommand.Create<string>(basePath =>
            {
                var exists = this.model.CheckDataBaseExists(basePath);

                if (exists)
                {
                    this.model.ShowSelectedBaseTab(basePath);
                }
                else
                {
                    BaseList.Remove(basePath);
                    DialogCoordinator.Instance
                        .ShowMessageAsync(this, "Ошибка", "База была удалена или перемещена из данной папки");
                }
            });

            NewDbCommand = ReactiveCommand.Create(() =>
            {
                var basePath = this.model.CreateDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                this.model.ShowSelectedBaseTab(basePath);
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
