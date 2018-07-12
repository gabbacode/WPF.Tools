using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using Data.Entities.Common.LocalBase;
using Kanban.Desktop.LocalBase.LocalBoard.Model;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.ViewModel
{
    public class LocalBoardViewModel : ViewModelBase, ILocalBoardViewModel//,IInitializableViewModel
    {

        private readonly ILocalBoardModel model;

        private readonly IDialogCoordinator dialogCoordinator = DialogCoordinator.Instance;

        [Reactive] private LocalIssue SelectedIssue  { get; set; }
        [Reactive] private RowInfo    SelectedRow    { get; set; }
        [Reactive] private ColumnInfo SelectedColumn { get; set; }

        [Reactive] public IDimension VerticalDimension { get; internal set; }

        [Reactive] public IDimension HorizontalDimension { get; internal set; }

        [Reactive] public ReactiveList<LocalIssue> Issues { get; internal set; }

        [Reactive] public ICardContent CardContent { get; private set; }

        public ReactiveCommand RefreshCommand { get; set; }

        public ReactiveCommand DeleteCommand { get; set; }

        public ReactiveCommand UpdateCommand { get; set; }

        public ReactiveCommand IssueSelectCommand { get; set; }

        public ReactiveCommand RowHeaderSelectCommand { get; set; }

        public ReactiveCommand ColumnHeaderSelectCommand { get; set; }


        public LocalBoardViewModel(ILocalBoardModel model)
        {
            this.model = model;

            Issues = new ReactiveList<LocalIssue>();
            
            RefreshCommand = ReactiveCommand.CreateFromTask(RefreshContent); //TODO: disable canrefresh when ui didn't complete

            var isSelectedEditable = this.WhenAnyValue(t => t.SelectedIssue, t => t.SelectedColumn, t => t.SelectedRow,
                (si, sc, sr) => si != null || sc != null || sr != null);

            DeleteCommand = ReactiveCommand.CreateFromTask(DeleteElement, isSelectedEditable);

            UpdateCommand = ReactiveCommand.Create(UpdateElement,isSelectedEditable);

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

                VerticalDimension = await model.GetRowHeadersAsync();

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

        private void UpdateElement()
        {
            if(SelectedIssue != null)
             model.ShowIssueView(SelectedIssue);

            else if (SelectedRow != null)
                 model.DeleteRowAsync(SelectedRow.Id);

            else if (SelectedColumn != null)
                 model.DeleteColumnAsync(SelectedColumn.Id);
        }

        public void Initialize(ViewRequest viewRequest)
        {
            Issues.Clear();

            Observable.FromAsync(() => model.GetRowHeadersAsync())
                .ObserveOnDispatcher()
                .Subscribe(vert =>VerticalDimension= vert);

            Observable.FromAsync(() => model.GetColumnHeadersAsync())
                .ObserveOnDispatcher()
                .Subscribe(horiz => HorizontalDimension = horiz);

            CardContent = model.GetCardContent();

            Observable.FromAsync(() => model.GetIssuesAsync())
                .ObserveOnDispatcher()
                .Subscribe(issues => Issues.AddRange(issues)); //can't work.
        }
    }
}
