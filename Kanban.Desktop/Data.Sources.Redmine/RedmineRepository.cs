using AutoMapper;
using Data.Sources.Common.Redmine;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Redmine.Net.Api.Async;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using CommonRemineEntities = Data.Entities.Common.Redmine;
using System.Threading.Tasks;
using Data.Sources.LocalStorage.Sqlite;

namespace Data.Sources.Redmine
{
    public class RedmineRepository : IRepository
    {
        public RedmineRepository()
        {
            EntityMapper = MappingBuilder.Build();

            ParametersMapping = new Dictionary<string, string>
            {
                { CommonRemineEntities.Keys.IssueId, RedmineKeys.ISSUE_ID },
                { CommonRemineEntities.Keys.ProjectId,  RedmineKeys.PROJECT_ID },
                { CommonRemineEntities.Keys.PriorityId, RedmineKeys.PRIORITY_ID },
                { CommonRemineEntities.Keys.StatusId, RedmineKeys.STATUS_ID },
                { CommonRemineEntities.Keys.TrackerId, RedmineKeys.TRACKER_ID },
                { CommonRemineEntities.Keys.AssignedToUserId, RedmineKeys.ASSIGNED_TO_ID },
            };

        }

        public void InitCredentials(string apiKey)
        {
            var redmineHost = ConfigurationManager.AppSettings["RedmineConnectionString"];

            RedmineManager = new RedmineManager(redmineHost, apiKey: apiKey);

        }

        public void InitCredentials(string username, string password)
        {
            var redmineHost = ConfigurationManager.AppSettings["RedmineConnectionString"];

            RedmineManager = new RedmineManager(redmineHost, username, password);
        }

        public CommonRemineEntities.User GetCurrentUser()
        {
            var currentUser = RedmineManager.GetCurrentUser();
            return EntityMapper.Map<CommonRemineEntities.User>(currentUser);
        }

        public CommonRemineEntities.Issue GetIssue(int id)
        {
            return GetIssueAsync(id).Result;
        }

        public async Task<CommonRemineEntities.Issue> GetIssueAsync(int id) 
        {
            var parameters = new NameValueCollection();
            parameters.Add(RedmineKeys.INCLUDE, RedmineKeys.JOURNALS);

            // TODO support multiple values
            var redmineIssue = await RedmineManager.GetObjectAsync<Issue>(id.ToString(), parameters);
            var issue = EntityMapper.Map<CommonRemineEntities.Issue>(redmineIssue);

            return issue;
        }

        public IEnumerable<CommonRemineEntities.Issue> GetIssues(NameValueCollection filters)
        {
            return GetIssuesAsync(filters).Result;

        }

        public async Task<IEnumerable<CommonRemineEntities.Issue>> GetIssuesAsync(
            NameValueCollection filters)
        {
            var parameters = new NameValueCollection();
            foreach (var key in filters.AllKeys)
            {
                if (ParametersMapping.TryGetValue(key, out string convertedKey))
                {
                    foreach (var value in filters.GetValues(key))
                    {
                        parameters.Add(convertedKey, value);
                    }
                }
            }

            // TODO support multiple values
            var redmineIssues = await RedmineManager.GetObjectsAsync<Issue>(parameters);

            var issues = EntityMapper.Map<List<CommonRemineEntities.Issue>>(redmineIssues);
            return issues;
        }

        public IEnumerable<CommonRemineEntities.Project> GetProjects()
        {
            return RedmineManager.GetObjects<Project>()
                .Select(p => EntityMapper.Map<CommonRemineEntities.Project>(p))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.Project>> GetProjectsAsync()
        {
            return (await RedmineManager.GetObjectsAsync<Project>(new NameValueCollection()))
                .Select(p => EntityMapper.Map<CommonRemineEntities.Project>(p))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.Priority> GetPriorities()
        {
            return RedmineManager.GetObjects<IssuePriority>()
                .Select(p => EntityMapper.Map<CommonRemineEntities.Priority>(p))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.Priority>> GetPrioritiesAsync()
        {
            return (await RedmineManager.GetObjectsAsync<IssuePriority>(new NameValueCollection()))
                .Select(p => EntityMapper.Map<CommonRemineEntities.Priority>(p))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.Status> GetStatuses()
        {
            return RedmineManager.GetObjects<IssueStatus>()
                .Select(s => EntityMapper.Map<CommonRemineEntities.Status>(s))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.Status>> GetStatusesAsync()
        {
            return (await RedmineManager.GetObjectsAsync<IssueStatus>(new NameValueCollection()))
                .Select(s => EntityMapper.Map<CommonRemineEntities.Status>(s))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.Tracker> GetTrackers(int projectId)
        {
            var filter = new NameValueCollection
            {
                { RedmineKeys.INCLUDE, RedmineKeys.TRACKERS }
            };

            return RedmineManager.GetObject<Project>(projectId.ToString(), filter).Trackers
                .Select(t => EntityMapper.Map<CommonRemineEntities.Tracker>(t))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.Tracker>> GetTrackersAsync(int projectId)
        {
            var filter = new NameValueCollection
            {
                { RedmineKeys.INCLUDE, RedmineKeys.TRACKERS }
            };

            return (await RedmineManager.GetObjectAsync<Project>(projectId.ToString(), filter)).Trackers
                .Select(t => EntityMapper.Map<CommonRemineEntities.Tracker>(t))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.User> GetUsers(int projectId)
        {
            var filter = new NameValueCollection
            {
                    {RedmineKeys.PROJECT_ID, projectId.ToString() }
            };

            return RedmineManager.GetObjects<ProjectMembership>(filter)
                .Select(pm => pm.User)
                .Select(u => EntityMapper.Map<CommonRemineEntities.User>(u))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.User>> GetUsersAsync(int projectId)
        {
            var filter = new NameValueCollection
            {
                    {RedmineKeys.PROJECT_ID, projectId.ToString() }
            };

            return (await RedmineManager.GetObjectsAsync<ProjectMembership>(filter))
                .Select(pm => pm.User)
                .Select(u => EntityMapper.Map<CommonRemineEntities.User>(u))
                .ToArray();

        }

        public async Task<CommonRemineEntities.Issue> CreateOrUpdateIssueAsync(CommonRemineEntities.Issue issue)
        {
            var redmineIssue = EntityMapper.Map<Issue>(issue);

            if (issue.Id.HasValue)
            {
                await RedmineManager.UpdateObjectAsync(redmineIssue.Id.ToString(), redmineIssue);
                return issue;
            }
            else
            {
                var newRedmineIssue = await RedmineManager.CreateObjectAsync(redmineIssue);
                return EntityMapper.Map<CommonRemineEntities.Issue>(newRedmineIssue);
            }
        }

        private RedmineManager RedmineManager;

        private IMapper EntityMapper;

        public Dictionary<string, string> ParametersMapping { get; }
    }
}
