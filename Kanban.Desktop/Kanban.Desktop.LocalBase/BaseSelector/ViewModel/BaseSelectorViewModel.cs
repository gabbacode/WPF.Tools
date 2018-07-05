using ReactiveUI;
using System.Reactive;
using MahApps.Metro.Controls.Dialogs;
using Kanban.Desktop.LocalBase.BaseSelector.Model;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.Common;
using Autofac;
using Data.Sources.LocalStorage.Sqlite;
using MahApps.Metro.Controls;
using System;
using System.Windows.Threading;

namespace Kanban.Desktop.LocalBase.BaseSelector.ViewModel
{
    public class BaseSelectorViewModel : ViewModelBase, IBaseSelectorViewModel
    {
        public ReactiveList<string>          BaseList            { get; set; }
        public Action                        CloseWindow         { get; set; }
        public string                        Title               { get; set; } = "BaseSelector";
        public ReactiveCommand               NewDbCommand        { get; set; }
        public ReactiveCommand               OpenDbCommand       { get; set; }
        public ReactiveCommand<string, Unit> OpenRecentDbCommand { get; set; }

        private readonly IShell _shell;
        private readonly IBaseSelectorModel _model;

        public BaseSelectorViewModel(IBaseSelectorModel model, IShell shell)
        {
            _model = model;
            _shell = shell;

            BaseList = new ReactiveList<string>();
            var list = _model.GetBaseList();
            foreach (var addr in list) BaseList.Add(addr);

            OpenRecentDbCommand = ReactiveCommand.Create<string>(basePath =>
            {
                var exists = _model.CheckRecentBase(basePath);
                if (exists)
                {
                    _shell.Container.Resolve<SqliteLocalRepository>(
                        new NamedParameter("baseName", basePath));
                    _shell.ShowStartView<IDockWindow>();
                    CloseWindow();
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

                _shell.Container.Resolve<SqliteLocalRepository>(
                    new NamedParameter("baseName", basePath));
                _shell.ShowStartView<IDockWindow>();
                CloseWindow();
            });

            OpenDbCommand = ReactiveCommand.Create(() =>
            {
                var basePath = _model.OpenDatabase();
                if (string.IsNullOrEmpty(basePath)) return;

                var t = _shell.Container.Resolve<SqliteLocalRepository>(
                    new NamedParameter("baseName", basePath));
                _shell.ShowStartView<IDockWindow>();
                CloseWindow();
            });
        }
    }
}
