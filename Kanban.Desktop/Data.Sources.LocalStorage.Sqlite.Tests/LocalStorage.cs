using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Data.Entities.Common.Redmine;
using Kanban.Desktop.LocalBase.SqliteLocalStorage;
using Kanban.Desktop.LocalBase.SqliteLocalStorage.Entities;
using NUnit.Framework;

namespace Data.Sources.LocalStorage.Sqlite.Tests
{
    [TestFixture]
    public class LocalStorage
    {

        private SqliteLocalRepository repos;

        public void old()
        {
           // _repos = new SqliteLocalRepository("dsa");
            var iss1 = new Issue()
            {
                CreatedOn = new DateTime(2, 2, 3),
                AssignedTo = new User() { Id = 5, Name = "usernumber5" },
                Description = "iss32ue 1 for unit test",
                Id = 2,
                Priority = new Priority() { Id = 63, Name = "i123sntPriority" },
                Project = new Project() { Id = 77, Name = "t32estsProj" },
                Status = new Status() { Id = 2224, Name = "2waitingforworking" },
                Subject = "very weak212 method",
                Tracker = new Tracker() { Id = 145, Name = "newTracker" },
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
                CreatedOn = new DateTime(2, 2, 3),
                AssignedTo = new User() { Id = 52, Name = "usernumber52" },
                Description = "iss32ue 1 for unit test",
                Id = 4,
                Priority = new Priority() { Id = 63, Name = "i123sntPriority" },
                Project = new Project() { Id = 77, Name = "t32estsProj" },
                Status = new Status() { Id = 2224, Name = "2waitingforworking" },
                Subject = "very weak212 method",
                Tracker = new Tracker() { Id = 145, Name = "newTracker" },
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
                CreatedOn = new DateTime(2, 2, 3),
                AssignedTo = new User() { Id = 52, Name = "usernumber52" },
                Description = "iss32ue 1 for unit test",
                Id = 7,
                Priority = new Priority() { Id = 63, Name = "i123sntPriority" },
                Project = new Project() { Id = 77, Name = "t32estsProj" },
                Status = new Status() { Id = 2224, Name = "2waitingforworking" },
                Subject = "very weak212 method",
                Tracker = new Tracker() { Id = 145, Name = "newTracker" },
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
                CreatedOn = new DateTime(2, 2, 3),
                //  AssignedTo  = new User() { Id = 18, Name = "newttestuser" },
                Description = "iss32ue 1 for unit test",
                Id = 18,
                Priority = new Priority() { Id = 162, Name = "i123sntPriority" },
                Project = new Project() { Id = 117, Name = "t32estsProj" },
                Status = new Status() { Id = 2124, Name = "2waitingforworking" },
                Subject = "very weak212 method",
                Tracker = new Tracker() { Id = 123, Name = "newTracker" },
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
        }
        //[Test]
        public async void RepShouldUpdateIssues()
        {

           // _repos = new SqliteLocalRepository("vsqwe");
            LocalIssue newiss = new LocalIssue()
            {
                //Id=3,
                Head="very fake issue",
                Body="fat body for fat issue",
                Color = "reed",
                Created = DateTime.Now,
                Modified = DateTime.Now,
                Row = new RowInfo { Name = "changingbysaving", Order = 2,Id=4},
                Column = new ColumnInfo { Name = "firstcolss", Order = 1,Id=3 },
            };

            // RowInfo newrow = new RowInfo { Name = "newrow", Order = 242, Width = 14 };
            var filt = new NameValueCollection();
            //var t=await _repos.CreateOrUpdateIssueAsync(newiss);
            //var t = _repos.GetIssues(filt);
            //var tt = t.Where(i => i.Id == 1);
            await repos.DeleteIssueAsync(17);
           // var newnew = _repos.CreateOrUpdateIssueAsync(newiss);

        }
    }
}
