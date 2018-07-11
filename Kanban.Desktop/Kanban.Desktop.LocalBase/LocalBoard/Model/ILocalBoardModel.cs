using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    public interface ILocalBoardModel
    {
        bool SaveOrUpdateColumn(ColumnInfo column);
        bool SaveOrUpdateRow(RowInfo row);


        IDimension              GetColumnHeaders();
        IDimension              GetRowHeaders();
        IEnumerable<LocalIssue> GetIssues();
        CardContent             GetCardContent();

        RowInfo                       GetSelectedRow(string rowName);
        ColumnInfo                    GetSelectedColumn(string colName);
        Task                          DeleteIssue(int issueId);
        Task                          DeleteRow(int rowId);
        Task                          DeleteColumn(int columnId);
        Task<IEnumerable<LocalIssue>> GetIssuesAsync();
    }
}
