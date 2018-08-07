using Data.Entities.Common.LocalBase;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Ui.Wpf.Common;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;
using Ui.Wpf.KanbanControl.Elements.CardElement;
using Autofac;
using Data.Sources.LocalStorage.Sqlite;

namespace Kanban.Desktop.LocalBase.Models
{
    // TODO: container for boards
    // TODO: local or server access
    public interface IScopeModel
    {
        Task<IDimension> GetColumnHeadersAsync();
        Task<IDimension> GetRowHeadersAsync();
        Task<IEnumerable<LocalIssue>> GetIssuesAsync();
        CardContent GetCardContent();
        RowInfo GetSelectedRow(string rowName);
        ColumnInfo GetSelectedColumn(string colName);

        Task DeleteIssueAsync(int issueId);
        Task DeleteRowAsync(int rowId);
        Task DeleteColumnAsync(int columnId);

        Task CreateOrUpdateColumnAsync(ColumnInfo column);
        Task CreateOrUpdateRowAsync(RowInfo row);

        Task<LocalIssue> LoadOrCreateIssueAsync(int? issueId);
        Task CreateOrUpdateIssueAsync(LocalIssue issue);
        Task<List<RowInfo>> GetRowsAsync();
        Task<List<ColumnInfo>> GetColumnsAsync();
    }

    public class ScopeModel : IScopeModel
    {
        private readonly SqliteLocalRepository repo;

        private List<RowInfo> rows = new List<RowInfo>();
        private List<ColumnInfo> columns = new List<ColumnInfo>();

        public ScopeModel(IShell shell, string uri)
        {
            repo = shell.Container.Resolve<SqliteLocalRepository>(
                new NamedParameter("conStr", uri));
        }

        #region GettingInfo

        public async Task<IDimension> GetColumnHeadersAsync()
        {
            columns.Clear();
            columns = await repo.GetColumnsAsync(1);

            var columnHeaders = columns.Select(c => c.Name).ToArray();
            return new TagDimension<string, LocalIssue>(
                tags: columnHeaders,
                getItemTags: i => new[] { i.Column.Name },
                categories: columnHeaders
                    .Select(c => new TagsDimensionCategory<string>(c, c))
                    .Select(tdc => (IDimensionCategory)tdc)
                    .ToArray());
        }

        public async Task<IDimension> GetRowHeadersAsync()
        {
            rows.Clear();
            rows = await repo.GetRowsAsync(1);

            var rowHeaders = rows.Select(r => r.Name).ToArray();
            return new TagDimension<string, LocalIssue>(
                tags: rowHeaders,
                getItemTags: i => new[] { i.Row.Name },
                categories: rowHeaders
                    .Select(r => new TagsDimensionCategory<string>(r, r))
                    .Select(tdc => (IDimensionCategory)tdc)
                    .ToArray()
            );
        }

        public async Task<IEnumerable<LocalIssue>> GetIssuesAsync()
        {
            return await repo.GetIssuesAsync(new NameValueCollection());
        }

        public CardContent GetCardContent()
        {
            return new CardContent(new ICardContentItem[]
            {
                new CardContentItem("Head"),
                new CardContentItem("Body"),
            });
        }

        public RowInfo GetSelectedRow(string rowName)
        {
            return rows.FirstOrDefault(r => r.Name == rowName);
        }

        public ColumnInfo GetSelectedColumn(string colName)
        {
            return columns.FirstOrDefault(c => c.Name == colName);
        }

        public async Task<List<RowInfo>> GetRowsAsync()
        {
            return await repo.GetRowsAsync(1);
        }

        public async Task<List<ColumnInfo>> GetColumnsAsync()
        {
            return await repo.GetColumnsAsync(1);
        }

        #endregion

        #region DeletingInfo

        public async Task DeleteIssueAsync(int issueId)
        {
            await repo.DeleteIssueAsync(issueId);
        }

        public async Task DeleteRowAsync(int rowId)
        {
            await repo.DeleteRowAsync(rowId);
        }

        public async Task DeleteColumnAsync(int columnId)
        {
            await repo.DeleteColumnAsync(columnId);
        }

        #endregion

        #region SavingInfo

        public async Task CreateOrUpdateColumnAsync(ColumnInfo column)
        {
            await repo.CreateOrUpdateColumnAsync(column);
        }

        public async Task CreateOrUpdateRowAsync(RowInfo row)
        {
            await repo.CreateOrUpdateRowAsync(row);
        }

        public async Task CreateOrUpdateIssueAsync(LocalIssue issue)
        {
            issue.Modified = DateTime.Now;
            await repo.CreateOrUpdateIssueAsync(issue);
        }
        

        #endregion

        public async Task<LocalIssue> LoadOrCreateIssueAsync(int? issueId)
        {
            var t = new LocalIssue();
            if (issueId.HasValue)
                t = await repo.GetIssueAsync(issueId.Value);

            return t;
        }
    }
}
