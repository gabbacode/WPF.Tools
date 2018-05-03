using Data.Entities.Common.Redmine;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Data.Sources.Common.Redmine
{
    public interface IRedmineRepository
    {
        void InitCredentials(string username, string password);

        User GetCurrentUser();

        IEnumerable<Issue> GetIssues(NameValueCollection filters);

        IEnumerable<Project> GetProjects();

        IEnumerable<Priority> GetPriorities();

        IEnumerable<Status> GetStatuses();

        IEnumerable<Tracker> GetTrackers();

        IEnumerable<User> GetUsers();
    }
}
