using Data.Entities.Common.Redmine;
using System.Collections.Generic;

namespace Data.Sources.Common.Redmine
{
    public interface IRedmineRepository
    {
        IEnumerable<Issue> GetIssues();

        IEnumerable<Project> GetProjects();

        IEnumerable<Status> GetStatuses();

        IEnumerable<Tracker> GetTrackers();

        IEnumerable<User> GetUsers();
    }
}
