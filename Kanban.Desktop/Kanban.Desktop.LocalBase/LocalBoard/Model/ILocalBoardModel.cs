using Data.Entities.Common.LocalBase;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    public interface ILocalBoardModel
    {
        bool SaveIssueState(LocalIssue issue);
        bool SaveOrUpdateColumn(ColumnInfo column);
        bool SaveOrUpdateRow(RowInfo row);
    }
}
