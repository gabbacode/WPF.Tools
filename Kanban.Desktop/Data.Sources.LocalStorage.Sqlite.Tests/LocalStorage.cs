using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Data.Entities.Common.Redmine;
using NUnit.Framework;

namespace Data.Sources.LocalStorage.Sqlite.Tests
{
    [TestFixture]
    public class LocalStorage
    {

        private SqliteLocalRepository _repos;

        [Test]
        public async void RepShouldUpdateIssues()
        {
            _repos = new SqliteLocalRepository();
            var iss1= new Issue()
                {
                    CreatedOn   = new DateTime(2, 2, 3),
                    AssignedTo  = new User() { Id =5, Name = "usernumber5" },
                    Description = "iss32ue 1 for unit test",
                    Id          = 2,
                    Priority    = new Priority() { Id =63, Name    = "i123sntPriority" },
                    Project     = new Project() { Id  = 77, Name = "t32estsProj" },
                    Status      = new Status() { Id   = 2224, Name  = "2waitingforworking" },
                    Subject     = "very weak212 method",
                    Tracker     = new Tracker() { Id = 145, Name = "newTracker" },
                    CustomFields = new List<CustomField>()
                    {
                        new CustomField()
                        {
                            Id   = 1,
                            Name = "cft1",
                            Values = new List<CustomFieldValue>()
                            {
                                new CustomFieldValue() {Value = "valuecft1"},
                                new CustomFieldValue() {Value = "oldvaluelol"}
                            }
                        }
                    }
                };

            var iss3 = new Issue()
            {
                CreatedOn   = new DateTime(2, 2, 3),
                AssignedTo  = new User() { Id = 52, Name = "usernumber52" },
                Description = "iss32ue 1 for unit test",
                Id          = 4,
                Priority    = new Priority() { Id = 63, Name   = "i123sntPriority" },
                Project     = new Project() { Id  = 77, Name   = "t32estsProj" },
                Status      = new Status() { Id   = 2224, Name = "2waitingforworking" },
                Subject     = "very weak212 method",
                Tracker     = new Tracker() { Id = 145, Name = "newTracker" },
                CustomFields = new List<CustomField>()
                {
                    new CustomField()
                    {
                        Id   = 6,
                        Name = "cft1",
                        Values = new List<CustomFieldValue>()
                        {
                            new CustomFieldValue() {Value = "valuecft1"},
                            new CustomFieldValue() {Value = "oldvaluelol"}
                        }
                    }
                }
            };

            var iss4 = new Issue()
            {
                CreatedOn   = new DateTime(2, 2, 3),
                AssignedTo  = new User() { Id = 52, Name = "usernumber52" },
                Description = "iss32ue 1 for unit test",
                Id          = 7,
                Priority    = new Priority() { Id = 63, Name   = "i123sntPriority" },
                Project     = new Project() { Id  = 77, Name   = "t32estsProj" },
                Status      = new Status() { Id   = 2224, Name = "2waitingforworking" },
                Subject     = "very weak212 method",
                Tracker     = new Tracker() { Id = 145, Name = "newTracker" },
                CustomFields = new List<CustomField>()
                {
                    new CustomField()
                    {
                        Id   = 1,
                        Name = "cft1",
                        Values = new List<CustomFieldValue>()
                        {
                            new CustomFieldValue() {Value = "valuecft1"},
                            new CustomFieldValue() {Value = "oldvaluelol"}
                        }
                    }
                }
            };

            var iss2 = new Issue()
            {
                CreatedOn   = new DateTime(2, 2, 3),
              //  AssignedTo  = new User() { Id = 18, Name = "newttestuser" },
                Description = "iss32ue 1 for unit test",
                Id          = 18,
                Priority    = new Priority() { Id = 162, Name  = "i123sntPriority" },
                Project     = new Project() { Id  = 117, Name   = "t32estsProj" },
                Status      = new Status() { Id   = 2124, Name = "2waitingforworking" },
                Subject     = "very weak212 method",
                Tracker     = new Tracker() { Id = 123, Name = "newTracker" },
                CustomFields = new List<CustomField>()
                {
                    new CustomField()
                    {
                        Id   = 1,
                        Name = "cft1",
                        Values = new List<CustomFieldValue>()
                        {
                            new CustomFieldValue() {Value = "valuecft1"},
                            new CustomFieldValue() {Value = "oldvaluelol"}
                        }
                    }
                }
            };
            _repos.InitCredentials("dasf", "422");
            var t = _repos.GetCurrentUser();
            //_repos.SaveIssue(iss2);
            _repos.SaveIssuesList(new List<Issue>() { iss2 });
            //await _repos.CreateOrUpdateIssueAsync(iss2);
            //var getIssueParameters = new NameValueCollection();
            //getIssueParameters.Add(Keys.IssueId, 7.ToString());
            //var tt = _repos.GetUsers(177).ToList();
            //var t= await _repos.GetUsersAsync(177);

        }

    }
}
