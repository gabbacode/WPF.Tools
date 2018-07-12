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
        private SqliteContext _context;
        private IMapper _mapper;
        public string BaseConnstr { get; set; }

        public SqliteLocalRepository(string baseName)
        {
            BaseConnstr = $"Data Source = {baseName}";
            _mapper = CreateMapper();
        }

        #region creating&updating entities
        public async Task<RowInfo> CreateOrUpdateRowAsync(RowInfo row)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                if (row.Id == 0 || _context.Row.Find(row.Id) == null)
                    await _context.AddAsync(row);

                else _context.Update(row);
                await _context.SaveChangesAsync();
                return row;
            }
        }

        public async Task<ColumnInfo> CreateOrUpdateColumnAsync(ColumnInfo column)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                if (column.Id == 0 || _context.Column.Find(column.Id) == null)
                    await _context.AddAsync(column);

                else _context.Update(column);
                await _context.SaveChangesAsync();
                return column;
            }
        }

        public async Task<LocalIssue> CreateOrUpdateIssueAsync(LocalIssue issue)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                var existed = _context.Issue
                    .AsNoTracking()
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .FirstOrDefault(iss => iss.Id == issue.Id);
                _mapper.Map(issue, existed);

                if (existed == null)
                {
                    var newiss = _mapper.Map<SqliteIssue>(issue);
                    _context.Attach(newiss.Row);
                    _context.Attach(newiss.Column);

                    await _context.AddAsync(newiss);
                    await _context.SaveChangesAsync();
                    _context.Update(newiss.Column);
                    _context.Update(newiss.Row);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<LocalIssue>(newiss);
                }
                else
                {
                    _context.Update(existed);
                    await _context.SaveChangesAsync();
                    return issue;
                }
            }
        }
        #endregion

        #region getting entities
        public List<LocalIssue> GetIssues(NameValueCollection filters)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                var dbIssues = _context.Issue
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .Select(i => _mapper.Map<LocalIssue>(i));

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
            using (_context = new SqliteContext(BaseConnstr))
            {
                var dbIssues = await _context.Issue
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .Select(i => _mapper.Map<LocalIssue>(i)).ToListAsync();

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
            using (_context = new SqliteContext(BaseConnstr))
            {
                return _context.Row.ToList();
            }
        }

        public async Task<List<RowInfo>> GetRowsAsync()
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                return await _context.Row.ToListAsync();
            }
        }

        public List<ColumnInfo> GetColumns()
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                return _context.Column.ToList();
            }
        }

        public async Task<List<ColumnInfo>> GetColumnsAsync()
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                return await _context.Column.ToListAsync();
            }
        }

        public async Task<LocalIssue> GetIssueAsync(int issueId)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                return await _context.Issue
                    .Include(i => i.Row)
                    .Include(i => i.Column)
                    .Select(i => _mapper.Map<LocalIssue>(i))
                    .FirstAsync(i=>i.Id==issueId);
            }
        }

        #endregion

        public async Task DeleteRowAsync(int? rowId)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                if (_context.Row.First().Id == rowId)
                {
                    var bindedIssues = _context.Issue
                        .Where(iss => iss.RowId == rowId);

                    var newxtRow = _context.Row.Skip(1).First();
                    foreach (var issue in bindedIssues)
                        issue.RowId = newxtRow.Id;

                    _context.UpdateRange(bindedIssues);
                    await _context.SaveChangesAsync();
                    _context.Row.Remove(_context.Row.Find(rowId));
                    await _context.SaveChangesAsync();
                    var t = _context.Issue.ToList();
                }

                else
                {
                    var bindedIssues = _context.Issue
                        .Where(iss => iss.RowId == rowId);
                    var previousRow = _context.Row.LastOrDefault(r => r.Id < rowId);
                    foreach (var issue in bindedIssues)
                        issue.RowId = previousRow.Id;

                    _context.UpdateRange(bindedIssues);
                    _context.Row.Remove(_context.Row.Find(rowId));
                    await _context.SaveChangesAsync();
                    var t = _context.Issue.ToList();
                }

            }
        }

        public async Task DeleteColumnAsync(int? columnId)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                if (_context.Column.First().Id == columnId)
                {
                    var bindedIssues = _context.Issue
                        .Where(iss => iss.ColumnId == columnId);

                    var newxtCol = _context.Column.Skip(1).First();
                    foreach (var issue in bindedIssues)
                        issue.ColumnId = newxtCol.Id;

                    _context.UpdateRange(bindedIssues);
                    await _context.SaveChangesAsync();
                    _context.Column.Remove(_context.Column.Find(columnId));
                    await _context.SaveChangesAsync();
                    var t = _context.Issue.ToList();
                }

                else
                {
                    var bindedIssues = _context.Issue
                        .Where(iss => iss.ColumnId == columnId);
                    var previousCol = _context.Column.LastOrDefault(r => r.Id < columnId);
                    foreach (var issue in bindedIssues)
                        issue.ColumnId = previousCol.Id;

                    _context.UpdateRange(bindedIssues);
                    _context.Column.Remove(_context.Column.Find(columnId));
                    await _context.SaveChangesAsync();
                    var t = _context.Issue.ToList();
                }

            }
        }

        public async Task DeleteIssueAsync(int? issueId)
        {
            using (_context = new SqliteContext(BaseConnstr))
            {
                _context.Issue
                    .Remove(_context.Issue.Find(issueId));
                await _context.SaveChangesAsync();
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