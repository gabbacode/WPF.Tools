using System.Collections.Generic;
using Data.Entities.Common.LocalBase;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    public interface ILocalBoardModel
    {
        bool                    SaveIssueState(LocalIssue issue);
        bool                    SaveOrUpdateColumn(ColumnInfo column);
        bool                    SaveOrUpdateRow(RowInfo row);
        IDimension              GetColumns();
        IDimension              GetRows();
        IEnumerable<LocalIssue> GetIssues();
        CardContent             GetCardContent();
    }
}
