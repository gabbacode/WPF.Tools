using System;
using System.Reactive;
using System.Reactive.Linq;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class WizardViewModel : ViewModelBase, IViewModel
    {
        [Reactive] public string BoardName { get; set; }
        [Reactive] public string FileName { get; set; }

        public ReactiveList<string> ColumnList { get; set; }
        public ReactiveList<string> RowList { get; set; }

        public ReactiveCommand CreateCommand { get; set; }
        public ReactiveCommand CancelCommand { get; set; }
        public ReactiveCommand<string, Unit> SelectFileCommand { get; set; }

        public ReactiveCommand AddColumnCommand { get; set; }
        public ReactiveCommand AddRowCommand { get; set; }

        public WizardViewModel()//StartupModel model)
        {
            Title = "Creating a board";

            this.WhenAnyValue(x => x.BoardName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(v => FileName = v);

            BoardName = "MyBoard";

            ColumnList = new ReactiveList<string>()
            {
                "Backlog",
                "In progress",
                "Done"
            };

            AddColumnCommand = ReactiveCommand.Create(() => ColumnList.Add("New column"));

            RowList = new ReactiveList<string>()
            {
                "Important",
                "So-so",
                "Trash"
            };

            AddRowCommand = ReactiveCommand.Create(() => RowList.Add("New row"));
        }
    }
}
