using System.Collections.Generic;
using System.Collections.Specialized;
using Data.Entities.Common.Redmine;

namespace Data.Sources.Common.Repository
{
    public interface IRepository
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
