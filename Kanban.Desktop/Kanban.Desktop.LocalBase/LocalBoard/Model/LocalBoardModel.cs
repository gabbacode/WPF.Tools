using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using Data.Sources.LocalStorage.Sqlite;
using Ui.Wpf.KanbanControl.Dimensions;
using Ui.Wpf.KanbanControl.Dimensions.Generic;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    public class LocalBoardModel : ILocalBoardModel
    {
        private readonly SqliteLocalRepository _repository;

        private List<RowInfo> _rows = new List<RowInfo>();

        private List<ColumnInfo> _columns = new List<ColumnInfo>();

        public LocalBoardModel(SqliteLocalRepository repository)
        {
            _repository = repository;
        }

        public IDimension GetColumnHeaders()
        {
            _columns.Clear();
            _columns = _repository.GetColumns();

            var columnHeaders = _columns.Select(c => c.Name).ToArray();
            return new TagDimension<string, LocalIssue>(
                tags: columnHeaders,
                getItemTags: i => new[] {i.Column.Name},
                categories: columnHeaders
                    .Select(c => new TagsDimensionCategory<string>(c, c))
                    .Select(tdc => (IDimensionCategory) tdc)
                    .ToArray());
        }

        public IDimension GetRowHeaders()
        {
            _rows.Clear();
            _rows = _repository.GetRows();

            var rowHeaders = _rows.Select(r => r.Name).ToArray();
            return new TagDimension<string, LocalIssue>(
                tags: rowHeaders,
                getItemTags: i => new[] {i.Row.Name},
                categories: rowHeaders
                    .Select(r => new TagsDimensionCategory<string>(r, r))
                    .Select(tdc => (IDimensionCategory) tdc)
                    .ToArray()
            );
        }

        public RowInfo GetSelectedRow(string rowName)
        {
            return _rows.FirstOrDefault(r=>r.Name==rowName);
        }

        public ColumnInfo GetSelectedColumn(string colName)
        {
            return _columns.FirstOrDefault(c => c.Name == colName);
        }

        public CardContent GetCardContent()
        {
            return new CardContent(new ICardContentItem[]
            {
                new CardContentItem("Head"),
                new CardContentItem("Body"),
            });
        }

        public async Task DeleteIssue(int issueId)
        {
            await _repository.DeleteIssueAsync(issueId);
        }

        public async Task DeleteRow(int rowId)
        {
            await _repository.DeleteRowAsync(rowId);
        }

        public async Task DeleteColumn(int columnId)
        {
            await _repository.DeleteColumnAsync(columnId);
        }

        public IEnumerable<LocalIssue> GetIssues()
        {
            return _repository.GetIssues(new NameValueCollection());
        }

        public async Task<IEnumerable<LocalIssue>> GetIssuesAsync()
        {
            var t = await _repository.GetIssuesAsync(new NameValueCollection());
            return t;
        }

        public bool SaveOrUpdateColumn(ColumnInfo column)
        {
            throw new NotImplementedException();
        }

        public bool SaveOrUpdateRow(RowInfo row)
        {
            throw new NotImplementedException();
        }
    }
}
