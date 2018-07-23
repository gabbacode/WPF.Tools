using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Forms;
using Kanban.Desktop.LocalBase.Models;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class WizardViewModel : ViewModelBase, IViewModel
    {
        [Reactive] public string BoardName { get; set; }
        [Reactive] public string FolderName { get; set; }
        [Reactive] public string FileName { get; set; }

        public ReactiveList<string> ColumnList { get; set; }
        public ReactiveList<string> RowList { get; set; }

        public ReactiveCommand CreateCommand { get; set; }
        public ReactiveCommand CancelCommand { get; set; }
        public ReactiveCommand SelectFolderCommand { get; set; }

        public ReactiveCommand AddColumnCommand { get; set; }
        public ReactiveCommand AddRowCommand { get; set; }

        private readonly IAppModel appModel_;

        public WizardViewModel(IAppModel appModel)
        {
            appModel_ = appModel;

            Title = "Creating a board";

            this.WhenAnyValue(x => x.BoardName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(v => FileName = BoardNameToFileName(v));

            /* TODO: Delayed check folder exists (Error)
             * this.WhenAnyValue(x => x.FolderName)
                .Throttle()
                .Subscribe();*/

            // TODO: Delayed check file exists (Warning)

            BoardName = "My Board";
            FolderName = "..";

            SelectFolderCommand = ReactiveCommand.Create(SelectFolder);

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

            CreateCommand = ReactiveCommand.Create(Create);
        }

        private string BoardNameToFileName(string boardName)
        {
            // stop chars for short file name    +=[]:;«,./?'space'
            // stops for long                    /\:*?«<>|

            char[] separators = new char[] {
                '+', '=', '[', ']', ':', ';', '"', ',', '.', '/', '?', ' ',
                '\\', '*', '<', '>', '|'};

            string str = boardName.Replace(separators, "_");
            return str + ".db";
        }

        public void SelectFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            //folderDialog.RootFolder = Environment.SpecialFolder.MyDocuments;

            if (dialog.ShowDialog() == DialogResult.OK)
                FolderName = dialog.SelectedPath;
        }

        public void Create()
        {
            appModel_.AddRecent(FolderName + "\\" + FileName);
            appModel_.Save();
        }
    }

    public static class ExtensionMethods
    {
        public static string Replace(this string s, char[] separators, string newVal)
        {
            string[] temp;

            temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            return String.Join(newVal, temp);
        }
    }
}
