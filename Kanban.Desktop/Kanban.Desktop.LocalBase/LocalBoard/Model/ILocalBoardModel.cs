using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    public interface ILocalBoardModel
    {
        Task<IDimension>              GetColumnHeadersAsync();
        Task<IDimension>              GetRowHeadersAsync();
        Task<IEnumerable<LocalIssue>> GetIssuesAsync();
        CardContent                   GetCardContent();
        RowInfo                       GetSelectedRow(string rowName);
        ColumnInfo                    GetSelectedColumn(string colName);

        Task DeleteIssueAsync(int issueId);
        Task DeleteRowAsync(int rowId);
        Task DeleteColumnAsync(int columnId);
        void ShowIssueView(LocalIssue issue);
    }
}
