using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;
using Data.Sources.LocalStorage.Sqlite.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;
// ReSharper disable All

namespace Data.Sources.LocalStorage.Sqlite
{
    public class SqliteLocalRepository : IRedmineRepository
    {
        private string connString;
        private SqliteContext _context;
        private IMapper _mapper;
        private User _currentUser;

        public SqliteLocalRepository()
        {
            //_context = context;
            _mapper = CreateMapper();
        }

        //public T GetEntityById<T>(object entityKey) where T : class
        //{
        //    if (typeof(T) == typeof(Issue))
        //    {
        //        var firstiss = _context.Issue.Find(entityKey);
        //        return _mapper.Map<T>(firstiss);
        //    }

        //    T entity = _context.Find<T>(entityKey);
        //    foreach (var refer in _context.Entry(entity).References)
        //    {
        //        refer.Load();
        //    }

        //    return entity;
        //}

        //public void SaveEntity<T>(T entity) where T : class
        //{
        //    if (typeof(T) == typeof(Issue))
        //    {
        //        //foreach (var ent in _context.Issue.Where(i=>i.Id>4405))
        //        //{
        //        //    _context.Issue.Remove(ent);
        //        //    _context.SaveChanges();
        //        //}
        //        //_context.User.Load();
        //        var isss = _mapper.Map<SqliteIssue>(entity);
        //        var existed = _context.Issue.Find(isss.Id);
        //        _mapper.Map<T, SqliteIssue>(entity, existed);
        //        if (existed == null)
        //        {
        //            try
        //            {
        //            _context.Entry(isss).State=EntityState.Added;
        //                _context.Add(isss);
        //                _context.SaveChanges();
        //            }
        //            catch (Exception e)
        //            {

        //            }

        //            var t = _context.Issue.ToList().First(i=>i.Id==isss.Id);
        //            foreach (var refer in _context.Entry(t).References)
        //            {
        //                refer.Load();
        //            }
        //        }
        //        else
        //        {
        //            //var itt = _context.Issue.Find(isss.Id);

        //            //foreach (var refer in _context.Entry(itt).References)
        //            //{
        //            //    refer.Load();
        //            //}
        //            //_context.Entry(existed).State = EntityState.Modified;
        //            //_context.Entry(itt).State = EntityState.Deleted;
        //            _context.Entry(existed.User).State = EntityState.Unchanged;
        //            _context.Entry(existed.Project).State = EntityState.Unchanged;
        //            _context.Entry(existed.Priority).State = EntityState.Unchanged;
        //            _context.Entry(existed.Status).State = EntityState.Unchanged;
        //            _context.Entry(existed.Tracker).State = EntityState.Unchanged;
        //            //_context.Entry(isss).State = EntityState.Added;
        //            //_context.Add(isss);
        //            _context.Update(existed);
        //            _context.SaveChanges();

        //            var t = _context.Issue.ToList().First(i=>i.Id==isss.Id);
        //            foreach (var refer in _context.Entry(t).References)
        //            {
        //                refer.Load();
        //            }
        //            //var tt = t.First(iss => iss.Id == isss.Id);
        //        }
        //        return;
        //    }

        //    if (_context.Find<T>(entity) == null)
        //    {
        //        {
        //            _context.Add(entity);
        //            _context.SaveChanges();
        //        }
        //    }
        //    else
        //    {
        //        _context.Update(entity);
        //        _context.SaveChanges();
        //    }
        //}

        //public void DeleteEntityById<T>(int entityId)
        //{
        //    throw new NotImplementedException();
        //}

        //public void GetAllInstances<T>()
        //{
        //    throw new NotImplementedException();
        //}

        public void SaveIssuesList(List<Issue> issues)
        {
            using (_context = new SqliteContext())
            {
                var newUsers = issues.Select(i => i.AssignedTo).ToList()
                    .Except(_context.User.AsNoTracking().ToList()).Where(i => i != null);
                _context.User.AddRange(newUsers);

                //var allUsers = issues.Select(iss => iss.AssignedTo)
                //    .GroupBy(u=>u.Id)
                //    .ToDictionary(g => g.Key, g => g.First());

                //foreach (var iss in issues)
                //{
                //    iss.AssignedTo = allUsers.First(au => au.Value.Id == iss.AssignedTo.Id).Value;
                //}

                var newProjects = issues.Select(i => i.Project).ToList()
                    .Except(_context.Project.AsNoTracking()).Where(i => i != null);
                _context.Project.AddRange(newProjects);

                var newPriors = issues.Select(i => i.Priority).ToList()
                    .Except(_context.Priority.AsNoTracking().ToList()).Where(i => i != null);
                _context.Priority.AddRange(newPriors);

                var newStatuses = issues.Select(i => i.Status).ToList()
                    .Except(_context.Status.AsNoTracking().ToList()).Where(i => i != null);
                _context.Status.AddRange(newStatuses);

                var newTrackers = issues.Select(i => i.Tracker).ToList()
                    .Except(_context.Tracker.AsNoTracking().ToList()).Where(i => i != null);
                _context.Tracker.AddRange(newTrackers);

                _context.SaveChanges();

                var sqlIssues = issues.Select(iss => _mapper.Map<SqliteIssue>(iss)).ToList();
                var dbIssues = _context.Issue
                    .AsNoTracking()
                    .Select(i => i.Id).ToList();

                var tt = sqlIssues.Where(i => dbIssues.Contains(i.Id)).ToList();
                if (tt.Count > 0)
                    _context.UpdateRange(tt);
                _context.SaveChanges();


                var t = sqlIssues.Except(tt).ToList();
                if (t.Count > 0)
                {
                    _context.AddRange(t);
                    _context.AttachRange(t.Select(iss => iss.User));
                    _context.AttachRange(t.Select(iss => iss.Project));
                    _context.AttachRange(t.Select(iss => iss.Priority));
                    _context.AttachRange(t.Select(iss => iss.Status));
                    _context.AttachRange(t.Select(iss => iss.Tracker));
                }
                _context.SaveChanges();
            }
        }

        public void InitCredentials(string username, string password)
        {
            using (_context = new SqliteContext())
            {
                _currentUser = _context.User
                    .Where(u => u.Name == username).First();
            }
        }

        public void InitCredentials(string apiKey)
        {
            using (_context = new SqliteContext())
            {
                _currentUser = _context.User
                    .First();
            }
        }

        public User GetCurrentUser()
        {
            return _currentUser;
        }

        public IEnumerable<Issue> GetIssues(NameValueCollection filters)
        {
           using (_context = new SqliteContext())
            {
                var dbIssues = _context.Issue
                    .Include(i => i.User)
                    .Include(i => i.Project)
                    .Include(i => i.Priority)
                    .Include(i => i.Tracker)
                    .Include(i => i.Status)
                    .Select(i=>_mapper.Map<Issue>(i));

                #region Keys

                var keys = filters.AllKeys;

                if (keys.Contains("IssueId"))
                    dbIssues = dbIssues
                        .Where(iss =>  filters.GetValues("IssueId")
                            .Contains(iss.Id.ToString()));

                if (keys.Contains("PriorityId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("PriorityId")
                            .Contains(iss.Priority.Id.ToString()));

                if (keys.Contains("ProjectId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("ProjectId")
                                .Contains(iss.Project.Id.ToString()));

                if (keys.Contains("StatusId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("StatusId")
                                .Contains(iss.Status.Id.ToString()));

                if (keys.Contains("TrackerId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("TrackerId")
                                .Contains(iss.Tracker.Id.ToString()));

                if (keys.Contains("AssignedToUserId"))
                    dbIssues = dbIssues
                        .Where(iss => filters.GetValues("AssignedToUserId")
                                .Contains(iss.AssignedTo.Id.ToString()));

                #endregion

                return dbIssues;
            }
        }

        public async Task<IEnumerable<Issue>> GetIssuesAsync(NameValueCollection filters)
        {
            using (_context = new SqliteContext())
            {
                var dbIssues = await _context.Issue
                    .Include(i => i.User)
                    .Include(i => i.Project)
                    .Include(i => i.Priority)
                    .Include(i => i.Tracker)
                    .Include(i => i.Status)
                    .Select(i => _mapper.Map<Issue>(i)).ToListAsync();

                #region Keys

                var keys = filters.AllKeys;

                IEnumerable<Issue> enumDbIssues=null;

                if (keys.Contains("IssueId"))
                    enumDbIssues = dbIssues
                        .Where(iss => filters.GetValues("IssueId")
                            .Contains(iss.Id.ToString()));

                if (keys.Contains("PriorityId"))
                    enumDbIssues = dbIssues
                        .Where(iss => filters.GetValues("PriorityId")
                            .Contains(iss.Priority.Id.ToString()));

                if (keys.Contains("ProjectId"))
                    enumDbIssues = dbIssues
                        .Where(iss => filters.GetValues("ProjectId")
                            .Contains(iss.Project.Id.ToString()));

                if (keys.Contains("StatusId"))
                    enumDbIssues = dbIssues
                        .Where(iss => filters.GetValues("StatusId")
                            .Contains(iss.Status.Id.ToString()));

                if (keys.Contains("TrackerId"))
                    enumDbIssues = dbIssues
                        .Where(iss => filters.GetValues("TrackerId")
                            .Contains(iss.Tracker.Id.ToString()));

                if (keys.Contains("AssignedToUserId"))
                    enumDbIssues = dbIssues
                        .Where(iss => filters.GetValues("AssignedToUserId")
                            .Contains(iss.AssignedTo.Id.ToString()));
                #endregion

                return enumDbIssues;
            }

        }

        public async Task<Issue> CreateOrUpdateIssueAsync(Issue issue)
        {
            using (_context = new SqliteContext())
            {
                var existed = _context.Issue.AsNoTracking().Where(iss => iss.Id == issue.Id).FirstOrDefault();
                _mapper.Map(issue, existed);

                if (existed == null)
                {
                    var newiss = _mapper.Map<SqliteIssue>(issue);

                    if (newiss.User != null &&  _context.User.Find(newiss.User.Id) == null)
                        await _context.User.AddAsync(newiss.User);

                    if (newiss.Project != null && _context.Project.Find(newiss.Project?.Id) == null)
                        await _context.Project.AddAsync(newiss.Project);

                    if (newiss.Priority != null && _context.Priority.Find(newiss.Priority?.Id) == null)
                        await _context.Priority.AddAsync(newiss.Priority);

                    if (newiss.Status != null && _context.Status.Find(newiss.Status?.Id) == null)
                        await _context.Status.AddAsync(newiss.Status);

                    if (newiss.Tracker != null && _context.Tracker.Find(newiss.Tracker?.Id) == null)
                        await _context.Tracker.AddAsync(newiss.Tracker);

                    await _context.AddAsync(newiss);
                    await _context.SaveChangesAsync();
                    return issue;
                }
                else
                {
                    if (existed.User != null && _context.User.AsNoTracking()
                            .Where(i => i.Id == existed.User.Id).FirstOrDefault() == null)
                       await _context.User.AddAsync(existed.User);

                    if (existed.Project != null && _context.Project.AsNoTracking()
                            .Where(i => i.Id == existed.Project.Id).FirstOrDefault() == null)
                        await _context.Project.AddAsync(existed.Project);

                    if (existed.Priority != null && _context.Priority.AsNoTracking()
                            .Where(i => i.Id == existed.Priority.Id).FirstOrDefault() == null)
                        await _context.Priority.AddAsync(existed.Priority);

                    if (existed.Status != null && _context.Status.AsNoTracking()
                            .Where(i => i.Id == existed.Status.Id).FirstOrDefault() == null)
                        await _context.Status.AddAsync(existed.Status);

                    if (existed.Tracker != null && _context.Tracker.AsNoTracking()
                            .Where(i => i.Id == existed.Tracker.Id).FirstOrDefault() == null)
                        await _context.Tracker.AddAsync(existed.Tracker);

                    _context.Update(existed);
                    await _context.SaveChangesAsync();
                    return issue;
                }
            }
        }
    
        public IEnumerable<Project> GetProjects()
        {
            using (_context = new SqliteContext())
            {
                return _context.Project.ToList().AsEnumerable();
            }
        }

        public async Task<IEnumerable<Project>> GetProjectsAsync()
        {
            using (_context = new SqliteContext())
            {
                var projects= await _context.Project.ToListAsync();
                return projects.AsEnumerable();
            }
        }

        public IEnumerable<Priority> GetPriorities()
        {
            using (_context = new SqliteContext())
            {
                return _context.Priority.ToList().AsEnumerable();
            }
        }

        public async Task<IEnumerable<Priority>> GetPrioritiesAsync()
        {
            using (_context = new SqliteContext())
            {
                var projects = await _context.Priority.ToListAsync();
                return projects.AsEnumerable();
            }
        }

        public IEnumerable<Status> GetStatuses()
        {
            using (_context = new SqliteContext())
            {
                return _context.Status.ToList().AsEnumerable();
            }
        }

        public async Task<IEnumerable<Status>> GetStatusesAsync()
        {
            using (_context = new SqliteContext())
            {
                var projects = await _context.Status.ToListAsync();
                return projects.AsEnumerable();
            }
        }

        public IEnumerable<Tracker> GetTrackers(int projectId)
        {
            using (_context = new SqliteContext())
            {
                var issues = _context.Issue.Include(i=>i.Tracker)
                    .Where(i => i.ProjectId == projectId).ToList();

                return issues.Select(i=>i.Tracker).Distinct();
            }
        }

        public async Task<IEnumerable<Tracker>> GetTrackersAsync(int projectId)
        {
            using (_context = new SqliteContext())
            {
                var issues = await _context.Issue.Include(i => i.Tracker)
                    .Where(i => i.ProjectId == projectId).ToListAsync();
                return issues.Select(i=>i.Tracker).Distinct();
            }
        }

        public IEnumerable<User> GetUsers(int projectId)
        {
            using (_context = new SqliteContext())
            {
                var issues = _context.Issue.Include(i => i.User)
                    .Where(i => i.ProjectId == projectId).ToList();

                return issues.Select(i => i.User).Distinct();
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync(int projectId)
        {
            using (_context = new SqliteContext())
            {
                var issues = await _context.Issue.Include(i => i.User)
                    .Where(i => i.ProjectId == projectId).ToListAsync();
                return issues.Select(i => i.User).Distinct();
            }
        }

        private IMapper CreateMapper()
        {
           var mapperConfig=new MapperConfiguration(cfg=>
               cfg.AddProfile(typeof(MapperProfileSqliteRepos)));
            return mapperConfig.CreateMapper();
        }

        public class MapperProfileSqliteRepos : Profile
        {
            public MapperProfileSqliteRepos()
            {
                CreateMap<Issue, SqliteIssue>()
                    .ForMember("Id", opt => opt.MapFrom(s => s.Id))
                    .ForMember("UserId", opt => opt.MapFrom(s => s.AssignedTo.Id))
                    .ForMember("ProjectId", opt => opt.MapFrom(s => s.Project.Id))
                    .ForMember("StatusId", opt => opt.MapFrom(s => s.Status.Id))
                    .ForMember("PriorityId", opt => opt.MapFrom(s => s.Priority.Id))
                    .ForMember("TrackerId", opt => opt.MapFrom(s => s.Tracker.Id))
                    .ForMember("User", opt => opt.MapFrom(s => s.AssignedTo))
                    .ForMember("CustomFields",
                        opt=>opt.MapFrom(s=>JsonConvert.SerializeObject(s.CustomFields)));

                CreateMap<SqliteIssue, Issue>()
                    .ForMember("AssignedTo", opt => opt.MapFrom(s => s.User))
                    .ForMember("CustomFields",
                        opt=>opt.MapFrom(s=> JsonConvert.DeserializeObject<IList<CustomField>>(s.CustomFields)));
            }
        }
    }
}