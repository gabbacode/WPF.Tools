using Data.Entities.Common.Redmine;
using System.Collections.Generic;

namespace Data.Sources.Common.Redmine
{
    public interface IRedmineRepository
    {
        void InitCredentials(string username, string password);

        User GetCurrentUser();

        IEnumerable<Issue> GetIssues(int? projectId = null);

        IEnumerable<Project> GetProjects();

        IEnumerable<Status> GetStatuses();

        IEnumerable<Tracker> GetTrackers();

        IEnumerable<User> GetUsers();
    }
}
