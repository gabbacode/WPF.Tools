using AutoMapper;
using Data.Sources.Common.Redmine;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using Redmine.Net.Api.Async;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using CommonRemineEntities = Data.Entities.Common.Redmine;
using System.Threading.Tasks;

namespace Data.Sources.Redmine
{
    public class RedmineRepository : IRedmineRepository
    {
        public RedmineRepository()
        {
            EntityMapper = MappingBuilder.Build();

            ParametersMapping = new Dictionary<string, string>
            {
                { CommonRemineEntities.Keys.IssueId, RedmineKeys.ISSUE_ID },
                { CommonRemineEntities.Keys.ProjectId,  RedmineKeys.PROJECT_ID },
                { CommonRemineEntities.Keys.PriorityId, RedmineKeys.PRIORITY_ID },
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

        public IEnumerable<CommonRemineEntities.Issue> GetIssues(NameValueCollection filters)
        {
            return GetIssuesAsync(filters).Result;
        }

        public async Task<IEnumerable<CommonRemineEntities.Issue>> GetIssuesAsync(NameValueCollection filters)
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

        public IEnumerable<CommonRemineEntities.Tracker> GetTrackers()
        {
            return RedmineManager.GetObjects<Tracker>()
                .Select(t => EntityMapper.Map<CommonRemineEntities.Tracker>(t))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.Tracker>> GetTrackersAsync()
        {
            return (await RedmineManager.GetObjectsAsync<Tracker>(new NameValueCollection()))
                .Select(t => EntityMapper.Map<CommonRemineEntities.Tracker>(t))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.User> GetUsers()
        {
            return RedmineManager.GetObjects<User>()
                .Select(u => EntityMapper.Map<CommonRemineEntities.User>(u))
                .ToArray();
        }

        public async Task<IEnumerable<CommonRemineEntities.User>> GetUsersAsync()
        {
            return (await RedmineManager.GetObjectsAsync<User>(new NameValueCollection()))
                .Select(u => EntityMapper.Map<CommonRemineEntities.User>(u))
                .ToArray();
        }

        public async Task<CommonRemineEntities.Issue> CreateOrUpdateIssueAsync(CommonRemineEntities.Issue issue)
        {
            var redmineIssue = EntityMapper.Map<CommonRemineEntities.Issue>(issue);

            if (issue.Id.HasValue)
            {
                await RedmineManager.UpdateObjectAsync(redmineIssue.Id.ToString(), redmineIssue);
                return issue;
            }
            else
            {
                return await RedmineManager.CreateObjectAsync(issue);
            }
        }

        private RedmineManager RedmineManager;

        private IMapper EntityMapper;

        public Dictionary<string, string> ParametersMapping { get; }
    }
}
