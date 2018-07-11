using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
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
    public class LocalBoardViewModel : ViewModelBase, ILocalBoardViewModel
    {

        private readonly ILocalBoardModel _model;

        private readonly IDialogCoordinator _dialogCoordinator = DialogCoordinator.Instance;

        private LocalIssue _selectedIssue;
        private RowInfo _selectedRow;
        private ColumnInfo _selectedColumn;

        [Reactive] public IDimension VerticalDimension { get; internal set; }

        [Reactive] public IDimension HorizontalDimension { get; internal set; }

        public ReactiveList<LocalIssue> Issues  { get; internal set; }

        [Reactive] public ICardContent CardContent { get; private set; }

        public ReactiveCommand RefreshCommand { get; set; }

        public ReactiveCommand DeleteCommand { get; set; }

        public ReactiveCommand IssueSelectCommand { get; set; }

        public ReactiveCommand RowSelectCommand { get; set; }

        public ReactiveCommand ColumnSelectCommand { get; set; }

        public LocalBoardViewModel(ILocalBoardModel model)
        {
            _model = model;

            

            Issues = new ReactiveList<LocalIssue>();

            RefreshCommand = ReactiveCommand.Create(RefreshContent);

            DeleteCommand = ReactiveCommand.CreateFromTask(Delete);

            IssueSelectCommand = ReactiveCommand.Create<object>(o =>
            {
                _selectedIssue = o as LocalIssue;

                if (_selectedIssue == null) return;

                _selectedColumn = null;
                _selectedRow    = null;
            });

            RowSelectCommand = ReactiveCommand.Create<object>(o =>
            {
                _selectedRow = _model.GetSelectedRow(o.ToString());

                if (_selectedRow == null) return;

                _selectedColumn = null;
                _selectedIssue  = null;
            });

            ColumnSelectCommand = ReactiveCommand.Create<object>(o =>
            {
                _selectedColumn = _model.GetSelectedColumn(o.ToString());

                if (_selectedColumn == null) return;

                _selectedRow   = null;
                _selectedIssue = null;
            });

            //Task.Factory.StartNew(() => RefreshContent());
        }

        private void RefreshContent()
        {
            Issues.Clear();

            VerticalDimension = _model.GetRowHeaders();

            HorizontalDimension = _model.GetColumnHeaders();

            CardContent = _model.GetCardContent();

            Issues.PublishCollection(_model.GetIssues());
        }

        private async Task Delete()
        {
            var ts = await _dialogCoordinator.ShowMessageAsync(this, "Warning",
                "Вы действительно хотите удалить данную запись?"
                , MessageDialogStyle.AffirmativeAndNegative);

            if (ts == MessageDialogResult.Negative)
                return;

                if (_selectedIssue != null)
                    await _model.DeleteIssue(_selectedIssue.Id);

                else if (_selectedRow != null)
                    await _model.DeleteRow(_selectedRow.Id);

                else if (_selectedColumn != null)
                    await _model.DeleteColumn(_selectedColumn.Id);

                RefreshContent();
        }
    }
}
