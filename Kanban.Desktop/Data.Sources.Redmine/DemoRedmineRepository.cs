using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Threading.Tasks;
using Data.Entities.Common.Redmine;
using Data.Sources.Common.Redmine;

namespace Data.Sources.Redmine
{
    public class DemoRedmineRepository : IRedmineRepository
    {
        public DemoRedmineRepository()
        {
            demoUsers = Enumerable
                .Range(0, 30)
                .Select(x => new User { Id = x, Name = $"user {x}" })
                .ToArray();
        }

        private readonly User[] demoUsers;

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
            new Tracker { Id = 2, Name = "bug" },

        };

        public User GetCurrentUser()
        {
            return demoUsers[0];
        }

        public IEnumerable<Issue> GetIssues(NameValueCollection filters)
        {
            var rnd = new Random();

            var issueId = 0;
            foreach (var user in demoUsers)
            {
                for (int i = 0; i < rnd.Next(3); i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        yield return new Issue
                        {
                            AssignedTo = user,
                            Id = --issueId,
                            Status = statuses[rnd.Next(statuses.Length)],
                            Description = $"long textual description of task{issueId} Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed eu egestas ligula, in dignissim eros. Etiam dolor dui, vulputate mollis massa vel, venenatis aliquam quam. Etiam eu massa eleifend, blandit ipsum eu, vestibulum orci. Ut auctor, risus ac semper commodo, libero dolor feugiat lorem, a cursus enim leo id nisi. In tempor condimentum blandit. ",
                            Priority = priorities[rnd.Next(priorities.Length)],
                            Project = projects[j],
                            Subject = $"subject of task{issueId}",
                            Tracker = trackers[rnd.Next(trackers.Length)],
                        };
                    }
                }
            }
        }

        public IEnumerable<Project> GetProjects()
        {
            return projects;
        }

        public IEnumerable<Priority> GetPriorities()
        {
            return priorities;
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

        public void InitCredentials(string username, string password)
        {
        }

        public void InitCredentials(string apiKey)
        {
        }

        public Task<IEnumerable<Issue>> GetIssuesAsync(NameValueCollection filters) => 
            Task.Run(() => GetIssues(filters));

        public Task<IEnumerable<Project>> GetProjectsAsync() =>
            Task.Run(() => GetProjects());

        public Task<IEnumerable<Priority>> GetPrioritiesAsync() =>
            Task.Run(() => GetPriorities());

        public Task<IEnumerable<Status>> GetStatusesAsync() =>
            Task.Run(() => GetStatuses());


        public Task<IEnumerable<Tracker>> GetTrackersAsync() =>
            Task.Run(() => GetTrackers());

        public Task<IEnumerable<User>> GetUsersAsync() =>
            Task.Run(() => GetUsers());
    }
}
