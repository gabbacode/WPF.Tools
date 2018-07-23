using Data.Entities.Common.LocalBase;
using Data.Sources.LocalStorage.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui.Wpf.Common;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;
using Ui.Wpf.KanbanControl.Elements.CardElement;
using Autofac;

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

        Task CreateOrUpdateColumn(ColumnInfo column);
        Task CreateOrUpdateRow(RowInfo row);
    }

    public class ScopeModel : IScopeModel
    {
        private readonly SqliteLocalRepository repo_;

        private List<RowInfo> rows = new List<RowInfo>();
        private List<ColumnInfo> columns = new List<ColumnInfo>();

        public ScopeModel(IShell shell, string uri)
        {
            repo_ = shell.Container.Resolve<SqliteLocalRepository>(
                new NamedParameter("conStr", uri));
        }

        #region GettingInfo

        public async Task<IDimension> GetColumnHeadersAsync()
        {
            columns.Clear();
            columns = await repo_.GetColumnsAsync();

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
            rows = await repo_.GetRowsAsync();

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
            return await repo_.GetIssuesAsync(new NameValueCollection());
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

        #endregion

        #region DeletingInfo

        public async Task DeleteIssueAsync(int issueId)
        {
            await repo_.DeleteIssueAsync(issueId);
        }

        public async Task DeleteRowAsync(int rowId)
        {
            await repo_.DeleteRowAsync(rowId);
        }

        public async Task DeleteColumnAsync(int columnId)
        {
            await repo_.DeleteColumnAsync(columnId);
        }

        #endregion

        public async Task CreateOrUpdateColumn(ColumnInfo column)
        {
            await repo_.CreateOrUpdateColumnAsync(column);
        }

        public async Task CreateOrUpdateRow(RowInfo row)
        {
            await repo_.CreateOrUpdateRowAsync(row);
        }
    }
}
