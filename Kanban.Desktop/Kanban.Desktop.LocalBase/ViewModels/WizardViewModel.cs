using System.Reactive;
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
        }
    }
}
