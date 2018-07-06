using ReactiveUI;
using System.Reactive;
using MahApps.Metro.Controls.Dialogs;
using Kanban.Desktop.LocalBase.BaseSelector.Model;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.BaseSelector.ViewModel
{
    public class BaseSelectorViewModel : ViewModelBase, IBaseSelectorViewModel
    {
        public ReactiveList<string>          BaseList            { get; set; }
        public ReactiveCommand               NewDbCommand        { get; set; }
        public ReactiveCommand               OpenDbCommand       { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly IBaseSelectorModel _model;

        public BaseSelectorViewModel(IBaseSelectorModel model)
        {
            _model = model;

            BaseList = new ReactiveList<string>();
            var list = _model.GetBaseList();
            foreach (var addr in list) BaseList.Add(addr);

            OpenRecentDbCommand = ReactiveCommand.Create<string>(basePath =>
            {
                var exists = _model.CheckRecentBase(basePath);

                if (exists)
                {
                    _model.ShowSelectedBaseTab(basePath);
                    Close();
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

                Close();
            });

            OpenDbCommand = ReactiveCommand.Create(() =>
            {
                var basePath = _model.OpenDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                _model.ShowSelectedBaseTab(basePath);

                Close();
            });
        }
    }
}
