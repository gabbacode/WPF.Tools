using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows.Forms;
using Data.Entities.Common.LocalBase;
using Kanban.Desktop.LocalBase.Models;
using Kanban.Desktop.LocalBase.Views;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Kanban.Desktop.LocalBase.WpfResources;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class WizardViewModel : ViewModelBase, IViewModel
    {
        [Reactive] public string BoardName { get; set; }
        [Reactive] public string FolderName { get; set; }
        [Reactive] public string FileName { get; set; }
        public ReactiveList<LocalDimension> ColumnList { get; set; }
        public ReactiveList<LocalDimension> RowList { get; set; }

        public ReactiveCommand CreateCommand { get; set; }
        public ReactiveCommand CancelCommand { get; set; }
        public ReactiveCommand SelectFolderCommand { get; set; }

        public ReactiveCommand AddColumnCommand { get; set; }
        public ReactiveCommand<int, Unit> DeleteColumnCommand { get; set; }
        public ReactiveCommand AddRowCommand { get; set; }
        public ReactiveCommand<int, Unit> DeleteRowCommand { get; set; }

        private readonly IAppModel appModel;
        private readonly IShell shell;

        public WizardViewModel(IAppModel appModel, IShell shell)
        {
            this.appModel = appModel;
            this.shell = shell;
            validator = new WizardValidator();

            Title = "Creating a board";

            this.WhenAnyValue(x => x.BoardName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(v => { FileName = BoardNameToFileName(v); });


            /* TODO: Delayed check folder exists (Error)
             * this.WhenAnyValue(x => x.FolderName)
                .Throttle()
                .Subscribe();*/

            // TODO: Delayed check file exists (Warning)

            BoardName = "My Board";
            FolderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            SelectFolderCommand = ReactiveCommand.Create(SelectFolder);

            ColumnList = new ReactiveList<LocalDimension>()
            {
                new LocalDimension("Backlog"),
                new LocalDimension("In progress"),
                new LocalDimension("Done")
            };

            AddColumnCommand =
                ReactiveCommand.Create(() => ColumnList.Add(new LocalDimension("New column")));

            DeleteColumnCommand = ReactiveCommand
                .Create<int>(index =>
                    ColumnList.Remove(ColumnList.First(column => column.Index == index)));

            RowList = new ReactiveList<LocalDimension>()
            {
                new LocalDimension("Important"),
                new LocalDimension("So-so"),
                new LocalDimension("Trash")
            };

            DeleteRowCommand = ReactiveCommand
                .Create<int>(index => RowList.Remove(RowList.First(row => row.Index == index)));

            AddRowCommand =
                ReactiveCommand.Create(() => RowList.Add(new LocalDimension("New row")));

            CreateCommand = ReactiveCommand.Create(Create);

            CancelCommand = ReactiveCommand.Create(Close);
        }

        private string BoardNameToFileName(string boardName)
        {
            // stop chars for short file name    +=[]:;«,./?'space'
            // stops for long                    /\:*?«<>|

            char[] separators =
            {
                '+', '=', '[', ']', ':', ';', '"', ',', '.', '/', '?', ' ',
                '\\', '*', '<', '>', '|'
            };

            string str = boardName.Replace(separators, "_");
            return str + ".db";
        }

        public void SelectFolder()
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.ShowNewFolderButton = false;
            //dialog.RootFolder = Environment.SpecialFolder.MyDocuments;
            dialog.SelectedPath = FolderName;

            if (dialog.ShowDialog() == DialogResult.OK)
                FolderName = dialog.SelectedPath;
        }

        public void Create()
        {
            string uri = FolderName + "\\"       + FileName;
            appModel.AddRecent(FolderName + "\\" + FileName);
            appModel.SaveConfig();

            var scope = appModel.CreateScope(uri);

            foreach (var colName in ColumnList.Select(column => column.Name))
                scope.CreateOrUpdateColumnAsync(new ColumnInfo {Name = colName});

            foreach (var rowName in RowList.Select(row => row.Name))
                scope.CreateOrUpdateRowAsync(new RowInfo {Name = rowName});

            Close();

            shell.ShowView<BoardView>(
                viewRequest: new BoardViewRequest {Scope = scope},
                options: new UiShowOptions {Title = FileName});
        }

        public class LocalDimension
        {
            private static int elementCount;

            public LocalDimension(string name)
            {
                Index = ++elementCount;
                Name = name;
            }

            [Reactive] public int Index { get; set; }
            [Reactive] public string Name { get; set; }
        }
    }

    public static class ExtensionMethods
    {
        public static string Replace(this string s, char[] separators, string newVal)
        {
            var temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);

            return String.Join(newVal, temp);
        }
    }
}