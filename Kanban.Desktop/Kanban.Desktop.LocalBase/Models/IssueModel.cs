using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using Data.Sources.LocalStorage.Sqlite;

namespace Kanban.Desktop.LocalBase.Issues.Model
{
    public class IssueModel
    {
        public IssueModel(SqliteLocalRepository repository)
        {
            sqliteRepository = repository;
        }

        public async Task<LocalIssue> LoadOrCreateAsync(int? issueId)
        {
            var t = new LocalIssue();
            if (issueId.HasValue)
                t = await sqliteRepository.GetIssueAsync(issueId.Value);

            return t;
        }

        public async Task SaveIssueAsync(LocalIssue issue)
        {
            issue.Modified = DateTime.Now;
            await sqliteRepository.CreateOrUpdateIssueAsync(issue);
        }

        public async Task<List<RowInfo>> GetRows()
        {
            return await sqliteRepository.GetRowsAsync();
        }

        public async Task<List<ColumnInfo>> GetColumns()
        {
            return await sqliteRepository.GetColumnsAsync();
        }

        private readonly SqliteLocalRepository sqliteRepository;
    }
}
