using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using GongSolutions.Wpf.DragDrop;
using Kanban.Desktop.LocalBase.LocalBoard.Model;
using MahApps.Metro.Controls.Dialogs;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.ViewModel
{
    public class LocalBoardViewModel : ViewModelBase, ILocalBoardViewModel //,IInitializableViewModel
    {

        private readonly ILocalBoardModel model;

        private readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;

        [Reactive] private LocalIssue SelectedIssue  { get; set; }
        [Reactive] private RowInfo    SelectedRow    { get; set; }
        [Reactive] private ColumnInfo SelectedColumn { get; set; }
        public IDropTarget LocalBoardHandler { get; set; } = new LocalBoardDropHandler();

        [Reactive] public IDimension VerticalDimension { get; internal set; }

        [Reactive] public IDimension HorizontalDimension { get; internal set; }


        public ReactiveList<string> Entities { get; } 
            = new ReactiveList<string>() { "Задачу", "Столбец", "Строку" };

        public ReactiveList<LocalIssue> Issues { get; internal set; }

        [Reactive] public ICardContent CardContent { get; private set; }

        public ReactiveCommand RefreshCommand { get; set; }

        public ReactiveCommand DeleteCommand { get; set; }

        public ReactiveCommand UpdateCommand { get; set; }

        public ReactiveCommand<string,Unit> AddNewElementCommand { get; set; }

        public ReactiveCommand IssueSelectCommand { get; set; }

        public ReactiveCommand RowHeaderSelectCommand { get; set; }

        public ReactiveCommand ColumnHeaderSelectCommand { get; set; }


        public LocalBoardViewModel(ILocalBoardModel model)
        {
            this.model = model;

            Issues = new ReactiveList<LocalIssue>();

            RefreshCommand =
                ReactiveCommand.CreateFromTask(RefreshContent); 

            var isSelectedEditable = this.WhenAnyValue(t => t.SelectedIssue, t => t.SelectedColumn, t => t.SelectedRow,
                (si, sc, sr) => si != null || sc != null || sr != null); //TODO :add selectcommand when click uneditable with nulling all "selected" fields

            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteElement, isSelectedEditable);

            UpdateCommand = ReactiveCommand.Create(UpdateElement, isSelectedEditable);

            AddNewElementCommand = ReactiveCommand.CreateFromTask<string>(async name=>await AddNewElement(name));

            IssueSelectCommand = ReactiveCommand.Create<object>(o =>
            {
                SelectedIssue = o as LocalIssue;

                if (SelectedIssue == null) return;

                SelectedColumn = null;
                SelectedRow    = null;
            });

            RowHeaderSelectCommand = ReactiveCommand.Create<object>(o =>
            {
                SelectedRow = this.model.GetSelectedRow(o.ToString());

                if (SelectedRow == null) return;

                SelectedColumn = null;
                SelectedIssue  = null;
            });

            ColumnHeaderSelectCommand = ReactiveCommand.Create<object>(o =>
            {
                SelectedColumn = this.model.GetSelectedColumn(o.ToString());

                if (SelectedColumn == null) return;

                SelectedRow   = null;
                SelectedIssue = null;
            });

        }

        private async Task RefreshContent()
        {
            Issues.Clear();

            VerticalDimension = null;
            VerticalDimension = await model.GetRowHeadersAsync();

            HorizontalDimension = null;
            HorizontalDimension = await model.GetColumnHeadersAsync();

            CardContent = model.GetCardContent();

            Issues.PublishCollection(await model.GetIssuesAsync());
        }

        private async Task DeleteElement()
        {
            var element = SelectedIssue != null ? "задачу" : SelectedColumn != null ? "весь столбец" : "всю строку";

            var ts = await dialogCoordinator.ShowMessageAsync(this, "Warning",
                $"Вы действительно хотите удалить {element}?"
                , MessageDialogStyle.AffirmativeAndNegative);

            if (ts == MessageDialogResult.Negative)
                return;

            if (SelectedIssue != null)
                await model.DeleteIssueAsync(SelectedIssue.Id);

            else if (SelectedRow != null)
                await model.DeleteRowAsync(SelectedRow.Id);

            else if (SelectedColumn != null)
                await model.DeleteColumnAsync(SelectedColumn.Id);

            await RefreshContent();
        }

        private async Task UpdateElement()
        {
            if (SelectedIssue != null)
                model.ShowIssueView(SelectedIssue);


            else if (SelectedRow != null)
            {
                var newName = await ShowRowNameInput();

                if (!string.IsNullOrEmpty(newName))
                {
                    SelectedRow.Name = newName;
                    await model.CreateOrUpdateRow(SelectedRow);
                }
            }


            else if (SelectedColumn != null)
            {
                var newName = await ShowColumnNameInput();

                if (!string.IsNullOrEmpty(newName))

                {
                    SelectedColumn.Name = newName;
                    await model.CreateOrUpdateColumn(SelectedColumn);
                }
            }

            await RefreshContent();
        }

        private async Task AddNewElement(string elementName) //TODO: add enum(?) as command parameter instead of string
        {
            if (elementName == "Задачу")
                    model.ShowIssueView(new LocalIssue());

            else if (elementName == "Строку")
            {
                var newName = await ShowRowNameInput();

                if (!string.IsNullOrEmpty(newName))
                {
                    var newRow = new RowInfo {Name = newName};
                    await model.CreateOrUpdateRow(newRow);
                }
            }

            else if (elementName == "Столбец")
            {
                var newName = await ShowColumnNameInput();

                if (!string.IsNullOrEmpty(newName))
                {
                    var newColumn = new ColumnInfo { Name = newName };
                    await model.CreateOrUpdateColumn(newColumn);
                }
            }

            await RefreshContent();
        }

        private async Task<string> ShowColumnNameInput()
        {
            return await dialogCoordinator
                .ShowInputAsync(this, "ColumnRed", "Введите название столбца",
                    new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "подтвердить",
                        NegativeButtonText    = "отмена",
                        DefaultText           = SelectedColumn?.Name
                    });
        }

        private async Task<string> ShowRowNameInput()
        {
            return await dialogCoordinator
                .ShowInputAsync(this, "RowRed", "Введите название строки",
                    new MetroDialogSettings()
                    {
                        AffirmativeButtonText = "подтвердить",
                        NegativeButtonText    = "отмена",
                        DefaultText           = SelectedRow?.Name,
                        DialogResultOnCancel = MessageDialogResult.Negative

                    });
        } //TODO: add some logic preventing empty name

        public void Initialize(ViewRequest viewRequest)
        {
            Issues.Clear();

            Observable.FromAsync(() => model.GetRowHeadersAsync())
                .ObserveOnDispatcher()
                .Subscribe(vert => VerticalDimension = vert);

            Observable.FromAsync(() => model.GetColumnHeadersAsync())
                .ObserveOnDispatcher()
                .Subscribe(horiz => HorizontalDimension = horiz);

            CardContent = model.GetCardContent();

            Observable.FromAsync(() => model.GetIssuesAsync())
                .ObserveOnDispatcher()
                .Subscribe(issues => Issues.AddRange(issues)); // TODO: make initialize works
        }


    }
}
