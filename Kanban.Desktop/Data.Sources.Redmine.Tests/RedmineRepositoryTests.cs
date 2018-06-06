﻿using AutoMapper;
using NUnit.Framework;
using Redmine.Net.Api.Types;
using CommonRemineEntities = Data.Entities.Common.Redmine;

namespace Data.Sources.Redmine.Tests
{
    [TestFixture]
    public class RedmineRepositoryTests
    {
        [Test]
        public void Issue_Should_Map_To_Redmine_Issue()
        {
            var redmineRepository = new RedmineRepository();
            var mapper = MappingBuilder.Build();

            var testIssue = MakeTestIssue();
            var readmineIssue = mapper.Map<Issue>(testIssue);
            var commonIssue = mapper.Map<CommonRemineEntities.Issue>(readmineIssue);
        }

        private  CommonRemineEntities.Issue MakeTestIssue()
        {
            var newIssue = new CommonRemineEntities.Issue
            {
                Subject = "Subject",
                Description = "Description",
                AssignedTo = new CommonRemineEntities.User() { Id = 153 },
                Project = new CommonRemineEntities.Project() { Id = 18 },
                Status = new CommonRemineEntities.Status() { Id = 1 },
                Tracker = new CommonRemineEntities.Tracker() { Id = 5 },
                Priority = new CommonRemineEntities.Priority() { Id = 2 },
                CustomFields = new[]
                {
                    MakeCustomValue(1, "PhoneNumber"),
                    MakeCustomValue(2, "ClientName"),
                    MakeCustomValue(5, "DeviceID"),
                    MakeCustomValue(11, "CarName"),
                    MakeCustomValue(23, "CarNumber"),
                    MakeCustomValue(25, "DateTime"),
                    MakeCustomValue(26, "ContractName"),
                    MakeCustomValue(27, ""),
                    MakeCustomValue(28, "да"),
                    MakeCustomValue(29, "PackageId"),
                }
            };

            return newIssue;
        }

        private CommonRemineEntities.CustomField MakeCustomValue(int fieldTypeId, string value)
        {
            return new CommonRemineEntities.CustomField
            {
                Id = fieldTypeId,
                Values = new[] { new CommonRemineEntities.CustomFieldValue { Value = value } }
            };
        }
    }
}
