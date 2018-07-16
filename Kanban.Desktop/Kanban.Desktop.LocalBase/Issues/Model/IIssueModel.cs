using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;

namespace Kanban.Desktop.LocalBase.Issues.Model
{
    public interface IIssueModel
    {
        Task<LocalIssue>       LoadOrCreateAsync(int? issueId);
        Task                   SaveIssueAsync(LocalIssue issue);
        Task<List<RowInfo>>    GetRows();
        Task<List<ColumnInfo>> GetColumns();
    }
}