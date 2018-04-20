using System;
using System.Collections.Generic;
using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;

namespace Data.Sources.Redmine
{
    public class DemoRedmineRepository : IRedmineRepository
    {
        private User[] demoUsers = 
        {
            new User { Id = 1, Name = "user 1" },
            new User { Id = 2, Name = "user 2" },
            new User { Id = 3, Name = "user 3" },
        };

        private Status[] statuses =
        {
            new Status { Id = 1, Name = "new" },
            new Status { Id = 2, Name = "solved" },
            new Status { Id = 3, Name = "in process" },
            new Status { Id = 4, Name = "feedback" },
        };

        private Priority[] priorities =
        {
            new Priority { Id = 1, Name = "low" },
            new Priority { Id = 2, Name = "normal" },
            new Priority { Id = 3, Name = "high" },
        };

        private Project[] projects =
        {
            new Project { Id = 1, Name = "project 1" },
            new Project { Id = 2, Name = "project 2" }
        };

        private Tracker[] trackers =
        {
            new Tracker { Id = 1, Name = "task" },
            new Tracker { Id = 1, Name = "bug" },

        };

        public IEnumerable<Issue> GetIssues()
        {
            var rnd = new Random();

            var issueId = 0;
            foreach (var user in demoUsers)
            {
                for (int i = 0; i < 3; i++)
                {
                    yield return new Issue
                    {
                        AssignedTo = user,
                        Id = --issueId,
                        Status = statuses[rnd.Next(statuses.Length - 1)],
                        Description = $"description of task{issueId}",
                        Priority = priorities[rnd.Next(priorities.Length - 1)],
                        Project = projects[0],
                        Subject = $"subject of task{issueId}",
                        Tracker = trackers[rnd.Next(trackers.Length - 1)],
                    };
                }
            }
        }

        public IEnumerable<Project> GetProjects()
        {
            return projects;
        }

        public IEnumerable<Status> GetStatuses()
        {
            return statuses;
        }

        public IEnumerable<Tracker> GetTrackers()
        {
            return trackers;
        }

        public IEnumerable<User> GetUsers()
        {
            return demoUsers;
        }
    }
}
