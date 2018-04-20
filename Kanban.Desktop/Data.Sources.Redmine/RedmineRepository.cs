using AutoMapper;
using Data.Sources.Common.Redmine;
using Redmine.Net.Api;
using Redmine.Net.Api.Types;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using CommonRemineEntities = Data.Entities.Common.Redmine;

namespace Data.Sources.Redmine
{
    public class RedmineRepository : IRedmineRepository
    {
        public RedmineRepository()
        {
            var redmineHost = ConfigurationManager.AppSettings["RedmineConnectionString"];
            RedmineManager = new RedmineManager(
                redmineHost,"x","x");


            EntityMapper = BuildMapper();
        }

        public IEnumerable<CommonRemineEntities.Issue> GetIssues()
        {
            var issues = RedmineManager.GetObjects<Issue>()
                .Select(i => new CommonRemineEntities.Issue
                {
                    Id = i.Id,
                    AssignedTo = EntityMapper.Map<CommonRemineEntities.User>(i.AssignedTo),
                    Description = i.Description,
                    Priority = EntityMapper.Map<CommonRemineEntities.Priority>(i.Priority),
                    Project = EntityMapper.Map<CommonRemineEntities.Project>(i.Project),
                    Status = EntityMapper.Map<CommonRemineEntities.Status>(i.Status),
                    Subject = i.Subject,
                    Tracker = EntityMapper.Map<CommonRemineEntities.Tracker>(i.Tracker)
                })
                .ToArray();

            return issues;
        }

        public IEnumerable<CommonRemineEntities.Project> GetProjects()
        {
            return RedmineManager.GetObjects<Project>()
                .Select(p => EntityMapper.Map<CommonRemineEntities.Project>(p))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.Status> GetStatuses()
        {
            return RedmineManager.GetObjects<IssueStatus>()
                .Select(s => EntityMapper.Map<CommonRemineEntities.Status>(s))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.Tracker> GetTrackers()
        {
            return RedmineManager.GetObjects<Tracker>()
                .Select(t => EntityMapper.Map<CommonRemineEntities.Tracker>(t))
                .ToArray();
        }

        public IEnumerable<CommonRemineEntities.User> GetUsers()
        {
            return RedmineManager.GetObjects<User>()
                .Select(u => EntityMapper.Map<CommonRemineEntities.User>(u))
                .ToArray();
        }

        private IMapper BuildMapper()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Issue, CommonRemineEntities.Issue>();
                cfg.CreateMap<IssuePriority, CommonRemineEntities.Priority>();
                cfg.CreateMap<Project, CommonRemineEntities.Project>();
                cfg.CreateMap<IssueStatus, CommonRemineEntities.Status>();
                cfg.CreateMap<Tracker, CommonRemineEntities.Tracker>();
                cfg.CreateMap<User, CommonRemineEntities.User>();
                cfg.CreateMap<IdentifiableName, CommonRemineEntities.User>();
            });

            return mapConfig.CreateMapper();
        }

        private RedmineManager RedmineManager;

        private IMapper EntityMapper;
    }
}
