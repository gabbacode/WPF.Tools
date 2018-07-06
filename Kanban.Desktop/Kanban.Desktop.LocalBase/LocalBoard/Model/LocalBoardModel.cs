using System;
using Data.Entities.Common.LocalBase;
using Data.Sources.LocalStorage.Sqlite;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    public class LocalBoardModel : ILocalBoardModel
    {
        private readonly SqliteLocalRepository _repository;

        public LocalBoardModel(SqliteLocalRepository repository)
        {
            _repository = repository;
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
