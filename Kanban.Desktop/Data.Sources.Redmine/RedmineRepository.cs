using Data.Sources.Common.Redmine;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Collections.Generic;
using System.Linq;

namespace Data.Sources.Redmine
{
    public class RedmineRepository : IRedmineRepository
    {
        public RedmineRepository()
        {
            RedmineManager = new RedmineManager(
                "x",
                "x",
                "x");
        }

        public IEnumerable<Entities.Common.Redmine.Issue> GetIssues()
        {
            return RedmineManager.GetObjects<Issue>()
                .Select(i => new Entities.Common.Redmine.Issue
                {
                    AssignedTo = i.AssignedTo != null
                        ? i.AssignedTo.Name
                        : i.Author.Name,
                    Project = i.Project.Name,
                    Subject = i.Subject,
                    Description = i.Description,
                    Priority = i.Priority.Name,
                    Status = i.Status.Name,
                    Tracker = i.Tracker.Name
                })
                .ToArray();
        }

        public IEnumerable<string> GetStatuses()
        {
            return RedmineManager.GetObjects<IssueStatus>()
                .Select(s => s.Name)
                .ToArray();
        }

        public IEnumerable<string> GetTrackers()
        {
            return RedmineManager.GetObjects<Tracker>()
                .Select(s => s.Name)
                .ToArray();
        }

        public IEnumerable<string> GetProjectTrackers()
        {
            return RedmineManager.GetObjects<ProjectTracker>()
                .Select(s => s.Name)
                .ToArray();
        }

        public IEnumerable<string> GetUsers()
        {
            return RedmineManager.GetObjects<User>()
                .Select(s => s.FirstName)
                .ToArray();
        }

        private RedmineManager RedmineManager;
    }
}
