using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Data.Entities.Common.LocalBase;
using FluentValidation;
using Kanban.Desktop.LocalBase.Models;
using Kanban.Desktop.LocalBase.Views;
using Kanban.Desktop.LocalBase.Views.WpfResources;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class WizardViewModel : ViewModelBase, IInitializableViewModel
    {
        [Reactive] public string BoardName { get; set; }
        [Reactive] public string FolderName { get; set; }
        [Reactive] public string FileName { get; set; }
        [Reactive] public bool CreateNewFile { get; set; }
        public ReactiveList<LocalDimension> ColumnList { get; set; }
        public ReactiveList<LocalDimension> RowList { get; set; }
        public ReactiveCommand CreateCommand { get; set; }
        public ReactiveCommand CancelCommand { get; set; }
        public ReactiveCommand SelectFolderCommand { get; set; }

        public ReactiveCommand AddColumnCommand { get; set; }
        public ReactiveCommand<LocalDimension, Unit> DeleteColumnCommand { get; set; }
        public ReactiveCommand AddRowCommand { get; set; }
        public ReactiveCommand<LocalDimension, Unit> DeleteRowCommand { get; set; }

        private readonly IAppModel appModel;
        private readonly IShell shell;

        public WizardViewModel(IAppModel appModel, IShell shell)
        {
            this.appModel = appModel;
            this.shell = shell;
            validator = new WizardValidator();
            Title = "Creating a board";
            CreateNewFile = true;

            this.WhenAnyValue(x => x.BoardName)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Subscribe(v =>
                {
                    if (CreateNewFile)
                        FileName = BoardNameToFileName(v);
                });

            BoardName = "My Board";

            FolderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            SelectFolderCommand = ReactiveCommand.Create(SelectFolder);

            ColumnList = new ReactiveList<LocalDimension>
            {
                new LocalDimension("Backlog"),
                new LocalDimension("In progress"),
                new LocalDimension("Done")
            };

            AddColumnCommand =
                ReactiveCommand.Create(() => ColumnList.Add(new LocalDimension("New column")));

            DeleteColumnCommand = ReactiveCommand
                .Create<LocalDimension>(column =>
                {
                    ColumnList.Remove(column);
                    UpdateDimensionList(ColumnList);
                });


            RowList = new ReactiveList<LocalDimension>()
            {
                new LocalDimension("Important"),
                new LocalDimension("So-so"),
                new LocalDimension("Trash")
            };

            AddRowCommand =
                ReactiveCommand.Create(() => RowList.Add(new LocalDimension("New row")));

            DeleteRowCommand = ReactiveCommand
                .Create<LocalDimension>(row =>
                {
                    RowList.Remove(row);
                    UpdateDimensionList(RowList);
                });

            var canCreate = this.WhenAnyValue(w => w.Error, string.IsNullOrEmpty);

            CreateCommand = ReactiveCommand.CreateFromTask(Create, canCreate);

            CancelCommand = ReactiveCommand.Create(Close);

            this.WhenAnyObservable(s => s.ColumnList.ItemChanged)
                .Subscribe(_ =>
                    UpdateDimensionList(ColumnList));

            this.WhenAnyObservable(s => s.RowList.ItemChanged)
                .Subscribe(_ =>
                    UpdateDimensionList(RowList));

            this.WhenAnyObservable(s => s.ColumnList.ItemsAdded)
                .Subscribe(_ => UpdateDimensionList(ColumnList));

            this.WhenAnyObservable(s => s.RowList.ItemsAdded)
                .Subscribe(_ => UpdateDimensionList(RowList));
        }

        private void UpdateDimensionList(ReactiveList<LocalDimension> list)
        {
            list.ChangeTrackingEnabled = false;
            foreach (var dim in list)
            {
                dim.IsDuplicate = false;
                dim.RaisePropertyChanged(nameof(dim.Name));
            }

            var duplicatgroups = list
                .GroupBy(dim => dim.Name)
                .Where(g => g.Count() > 1)
                .ToList();


            foreach (var group in duplicatgroups)
            {
                foreach (var dim in group)
                {
                    dim.IsDuplicate = true;
                    dim.RaisePropertyChanged(nameof(dim.Name));
                }
            }

            list.ChangeTrackingEnabled = true;
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
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                SelectedPath = FolderName
            };
            //dialog.RootFolder = Environment.SpecialFolder.MyDocuments;

            if (dialog.ShowDialog() == DialogResult.OK)
                FolderName = dialog.SelectedPath;
        }

        public async Task Create()
        {
            string uri = FolderName + "\\"       + FileName;
            appModel.AddRecent(FolderName + "\\" + FileName);
            appModel.SaveConfig();

            var scope = appModel.CreateScope(uri);

            var newBoard = new BoardInfo()
            {
                Name = BoardName,
                Created = DateTime.Now,
                Modified = DateTime.Now
            };

            newBoard = await scope.CreateOrUpdateBoardAsync(newBoard);

            foreach (var colName in ColumnList.Select(column => column.Name))
                await scope.CreateOrUpdateColumnAsync(new ColumnInfo
                {
                    Name = colName,
                    Board = newBoard
                });

            foreach (var rowName in RowList.Select(row => row.Name))
                await scope.CreateOrUpdateRowAsync(new RowInfo {Name = rowName, Board = newBoard});

            Close();

            shell.ShowView<BoardView>(
                viewRequest: new BoardViewRequest {Scope = scope,SelectedBoardName = BoardName},
                options: new UiShowOptions {Title = FileName});
        }

        public void Initialize(ViewRequest viewRequest)
        {
            var request = viewRequest as WizardViewRequest;

            CreateNewFile = (bool) request?.CreateNewFile;

            if (!CreateNewFile)
            {
                FolderName = Path.GetDirectoryName(request.Uri);
                FileName = Path.GetFileName(request.Uri);
            }
        }

        public class LocalDimension : ViewModelBase, IDataErrorInfo
        {
            public LocalDimension(string name)
            {
                Name = name;
                validator = new LocalDimensionValidator();
            }

            public bool IsDuplicate { get; set; }
            [Reactive] public string Name { get; set; }

            public new IValidator validator;

            public new string Error
            {
                get
                {
                    var results = validator?.Validate(this);

                    if (results != null && results.Errors.Any())
                    {
                        var errors = string.Join(Environment.NewLine,
                            results.Errors.Select(x => x.ErrorMessage).ToArray());
                        return errors;
                    }

                    return string.Empty;
                }
            }

            public new string this[string columnName]
            {
                get
                {
                    var errs = validator?
                        .Validate(this).Errors;

                    if (errs != null)
                        return validator != null
                            ? string.Join("; ", errs.Select(e => e.ErrorMessage))
                            : "";
                    return "";
                }
            }

        } //class
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
