using Data.Entities.Common.Redmine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Data.Sources.Common.Redmine
{
    public interface IRepository
    {
        void InitCredentials(string username, string password);

        void InitCredentials(string apiKey);

        User GetCurrentUser();

        Issue GetIssue(int id);

        Task<Issue> GetIssueAsync(int id);

        IEnumerable<Issue> GetIssues(NameValueCollection filters);

        Task<IEnumerable<Issue>> GetIssuesAsync(NameValueCollection filters);

        Task<Issue> CreateOrUpdateIssueAsync(Issue issue);

        IEnumerable<Project> GetProjects();

        Task<IEnumerable<Project>> GetProjectsAsync();

        IEnumerable<Priority> GetPriorities();

        Task<IEnumerable<Priority>> GetPrioritiesAsync();

        IEnumerable<Status> GetStatuses();

        Task<IEnumerable<Status>> GetStatusesAsync();

        IEnumerable<Tracker> GetTrackers(int projectId);

        Task<IEnumerable<Tracker>> GetTrackersAsync(int projectId);

        IEnumerable<User> GetUsers(int projectId);

        Task<IEnumerable<User>> GetUsersAsync(int projectId);
    }
}
