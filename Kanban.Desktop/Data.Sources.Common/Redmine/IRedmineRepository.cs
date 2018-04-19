using Data.Entities.Common.Redmine;
using System.Collections.Generic;

namespace Data.Sources.Common.Redmine
{
    public interface IRedmineRepository
    {
        IEnumerable<Issue> GetIssues();

        IEnumerable<string> GetStatuses();

        IEnumerable<string> GetTrackers();

        IEnumerable<string> GetProjectTrackers();

        IEnumerable<string> GetUsers();
    }
}
