using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entities.Common.LocalBase;
using Data.Sources.LocalStorage.Sqlite.Context;
using Microsoft.EntityFrameworkCore;

namespace Data.Sources.LocalStorage.Sqlite
{
    public class SqliteLocalRepository
    {
        private SqliteContext context;
        private readonly IMapper mapper;
        public string BaseConnstr { get; set; }

        public SqliteLocalRepository(IDataBaseSettings settings)
        {
            BaseConnstr = settings.GetConnectionString();
            mapper = CreateMapper();
        }

        #region creating&updating entities
        public async Task<RowInfo> CreateOrUpdateRowAsync(RowInfo row)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                if (row.Id == 0 || context.Row.Find(row.Id) == null)
                    await context.AddAsync(row);

                else context.Update(row);
                await context.SaveChangesAsync();
                return row;
            }
        }

        public async Task<ColumnInfo> CreateOrUpdateColumnAsync(ColumnInfo column)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                if (column.Id == 0 || context.Column.AsNoTracking()
                        .FirstOrDefault(c => c.Id == column.Id) == null)
                    await context.AddAsync(column);


                else context.Update(column);
                await context.SaveChangesAsync();
                return column;
            }
        }

        public async Task<LocalIssue> CreateOrUpdateIssueAsync(LocalIssue issue)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                var existed = context.Issue
                    .AsNoTracking()
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .FirstOrDefault(iss => iss.Id == issue.Id);
                mapper.Map(issue, existed);

                if (existed == null)
                {
                    var newiss = mapper.Map<SqliteIssue>(issue);
                    context.Attach(newiss.Row);
                    context.Attach(newiss.Column);

                    await context.AddAsync(newiss);
                    await context.SaveChangesAsync();
                    context.Update(newiss.Column);
                    context.Update(newiss.Row);
                    await context.SaveChangesAsync();
                    return mapper.Map<LocalIssue>(newiss);
                }

                context.Update(existed);
                await context.SaveChangesAsync();
                return issue;
            }
        }
        #endregion

        #region getting entities
        public List<LocalIssue> GetIssues(NameValueCollection filters)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                var dbIssues = context.Issue
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .Select(i => mapper.Map<LocalIssue>(i));

                var keys = filters.AllKeys;

                if (keys.Contains("IssueId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("IssueId")
                            .Contains(iss.Id.ToString()));

                if (keys.Contains("RowId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("RowId")
                            .Contains(iss.Row.Id.ToString()));

                if (keys.Contains("ColumnId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("ColumnId")
                            .Contains(iss.Column.Id.ToString()));

                return dbIssues.ToList();
            }
        }

        public async Task<List<LocalIssue>> GetIssuesAsync(NameValueCollection filters)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                var dbIssues = await context.Issue
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .Select(i => mapper.Map<LocalIssue>(i)).ToListAsync();

                var keys = filters.AllKeys;

                if (keys.Contains("IssueId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("IssueId")
                            .Contains(iss.Id.ToString())).ToList();

                if (keys.Contains("RowId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("RowId")
                            .Contains(iss.Row.Id.ToString())).ToList();

                if (keys.Contains("ColumnId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("ColumnId")
                            .Contains(iss.Column.Id.ToString())).ToList();

                return dbIssues;
            }

        }

        public List<RowInfo> GetRows()
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                return context.Row.ToList();
            }
        }

        public async Task<List<RowInfo>> GetRowsAsync()
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                return await context.Row.ToListAsync();
            }
        }

        public List<ColumnInfo> GetColumns()
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                return context.Column.ToList();
            }
        }

        public async Task<List<ColumnInfo>> GetColumnsAsync()
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                return await context.Column.ToListAsync();
            }
        }

        public async Task<LocalIssue> GetIssueAsync(int issueId)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                return await context.Issue
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .Select(i => mapper.Map<LocalIssue>(i))
                    .FirstAsync(i=>i.Id==issueId);
            }
        }

        #endregion

        public async Task DeleteRowAsync(int? rowId)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                if (context.Row.First().Id == rowId)
                {
                    var bindedIssues = context.Issue
                        .Where(iss => iss.RowId == rowId);

                    var newxtRow = context.Row.Skip(1).First();
                    foreach (var issue in bindedIssues)
                        issue.RowId = newxtRow.Id;

                    context.UpdateRange(bindedIssues);
                    await context.SaveChangesAsync();
                    context.Row.Remove(context.Row.Find(rowId));
                    await context.SaveChangesAsync();
                    var t = context.Issue.ToList();
                }

                else
                {
                    var bindedIssues = context.Issue
                        .Where(iss => iss.RowId == rowId);
                    var previousRow = context.Row.LastOrDefault(r => r.Id < rowId);
                    foreach (var issue in bindedIssues)
                        issue.RowId = previousRow.Id;

                    context.UpdateRange(bindedIssues);
                    context.Row.Remove(context.Row.Find(rowId));
                    await context.SaveChangesAsync();
                    var t = context.Issue.ToList();
                }

            }
        }

        public async Task DeleteColumnAsync(int? columnId)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                if (context.Column.First().Id == columnId)
                {
                    var bindedIssues = context.Issue
                        .Where(iss => iss.ColumnId == columnId);

                    var newxtCol = context.Column.Skip(1).First();
                    foreach (var issue in bindedIssues)
                        issue.ColumnId = newxtCol.Id;

                    context.UpdateRange(bindedIssues);
                    await context.SaveChangesAsync();
                    context.Column.Remove(context.Column.Find(columnId));
                    await context.SaveChangesAsync();
                    var t = context.Issue.ToList();
                }

                else
                {
                    var bindedIssues = context.Issue
                        .Where(iss => iss.ColumnId == columnId);
                    var previousCol = context.Column.LastOrDefault(r => r.Id < columnId);
                    foreach (var issue in bindedIssues)
                        issue.ColumnId = previousCol.Id;

                    context.UpdateRange(bindedIssues);
                    context.Column.Remove(context.Column.Find(columnId));
                    await context.SaveChangesAsync();
                    var t = context.Issue.ToList();
                }

            }
        }

        public async Task DeleteIssueAsync(int? issueId)
        {
            using (context = new SqliteContext(BaseConnstr))
            {
                context.Issue
                    .Remove(context.Issue.Find(issueId));
                await context.SaveChangesAsync();
            }

        }

        private IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
                  cfg.AddProfile(typeof(MapperProfileSqliteRepos)));
            return mapperConfig.CreateMapper();
        }

        public class MapperProfileSqliteRepos : Profile
        {
            public MapperProfileSqliteRepos()
            {
                CreateMap<LocalIssue, SqliteIssue>()
                    .ForMember("RowId", opt => opt.MapFrom(s => s.Row.Id))
                    .ForMember("ColumnId", opt => opt.MapFrom(s => s.Column.Id));

                CreateMap<SqliteIssue, LocalIssue>();
            }
        }
    }
}