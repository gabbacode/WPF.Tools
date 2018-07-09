using System.Reactive;
using Kanban.Desktop.LocalBase.DataBaseSelector.Model;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.DataBaseSelector.ViewModel
{
    public class BaseSelectorViewModel : ViewModelBase, IBaseSelectorViewModel
    {
        public ReactiveList<string>          BaseList            { get; set; }
        public ReactiveCommand               NewDbCommand        { get; set; }
        public ReactiveCommand               OpenDbCommand       { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly IDataBaseSelectorModel _model;

        public BaseSelectorViewModel(IDataBaseSelectorModel model)
        {
            _model = model;

            BaseList = new ReactiveList<string>();
            var list = _model.GetBaseList();
            foreach (var addr in list) BaseList.Add(addr);

            OpenRecentDbCommand = ReactiveCommand.Create<string>(basePath =>
            {
                var exists = _model.CheckDataBaseExists(basePath);

                if (exists)
                {
                    _model.ShowSelectedBaseTab(basePath);
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
                var basePath = _model.CreateDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                _model.ShowSelectedBaseTab(basePath);
            });

            OpenDbCommand = ReactiveCommand.Create(() =>
            {
                var basePath = _model.OpenDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                _model.ShowSelectedBaseTab(basePath);
            });
        }
    }
}
