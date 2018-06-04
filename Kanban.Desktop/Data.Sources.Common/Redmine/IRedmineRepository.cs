using Data.Entities.Common.Redmine;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Data.Sources.Common.Redmine
{
    public interface IRedmineRepository
    {
        void InitCredentials(string username, string password);

        void InitCredentials(string apiKey);

        User GetCurrentUser();

        IEnumerable<Issue> GetIssues(NameValueCollection filters);

        Task<IEnumerable<Issue>> GetIssuesAsync(NameValueCollection filters);

        IEnumerable<Project> GetProjects();

        Task<IEnumerable<Project>> GetProjectsAsync();

        IEnumerable<Priority> GetPriorities();

        Task<IEnumerable<Priority>> GetPrioritiesAsync();

        IEnumerable<Status> GetStatuses();

        Task<IEnumerable<Status>> GetStatusesAsync();

        IEnumerable<Tracker> GetTrackers();

        Task<IEnumerable<Tracker>> GetTrackersAsync();

        IEnumerable<User> GetUsers();

        Task<IEnumerable<User>> GetUsersAsync();
    }
}
