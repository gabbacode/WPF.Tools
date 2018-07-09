using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
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

        public LocalBoardModel(SqliteLocalRepository repository)
        {
            _repository = repository;
        }

        public IDimension GetColumns()
        {
            var columns = _repository.GetColumns().Select(c => c.Name).ToArray();
            return new TagDimension<string, LocalIssue>(
                tags: columns,
                getItemTags: i => new[] {i.Column.Name},
                categories: columns
                    .Select(c => new TagsDimensionCategory<string>(c, c))
                    .Select(tdc => (IDimensionCategory) tdc)
                    .ToArray());
        }

        public IDimension GetRows()
        {
            var rows = _repository.GetRows().Select(r => r.Name).ToArray();
            return new TagDimension<string, LocalIssue>(
                tags: rows,
                getItemTags: i => new[] {i.Row.Name},
                categories: rows
                    .Select(r => new TagsDimensionCategory<string>(r, r))
                    .Select(tdc => (IDimensionCategory) tdc)
                    .ToArray()
            );
        }

        public CardContent CardContent()
        {
            return new CardContent(new ICardContentItem[]
            {
                new CardContentItem("Head"),
                new CardContentItem("Body"),
            });
        }

        public IEnumerable<LocalIssue> GetIssues()
        {
            return _repository.GetIssues(new NameValueCollection());
        }

        public bool SaveIssueState(LocalIssue issue)
        {
            throw new NotImplementedException();
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
