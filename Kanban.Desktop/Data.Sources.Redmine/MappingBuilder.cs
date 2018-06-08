using System.Linq;
using AutoMapper;
using Redmine.Net.Api.Types;
using CommonRemineEntities = Data.Entities.Common.Redmine;

namespace Data.Sources.Redmine
{
    public class MappingBuilder
    {
        public static IMapper Build()
        {
            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Issue, CommonRemineEntities.Issue>();
                cfg.CreateMap<CommonRemineEntities.Issue, Issue>()
                    .ForMember(x => x.Category, opt => opt.Ignore())
                    .ForMember(x => x.StartDate, opt => opt.Ignore())
                    .ForMember(x => x.DueDate, opt => opt.Ignore())
                    .ForMember(x => x.DoneRatio, opt => opt.Ignore())
                    .ForMember(x => x.PrivateNotes, opt => opt.Ignore())
                    .ForMember(x => x.EstimatedHours, opt => opt.Ignore())
                    .ForMember(x => x.SpentHours, opt => opt.Ignore())
                    .ForMember(x => x.UpdatedOn, opt => opt.Ignore())
                    .ForMember(x => x.ClosedOn, opt => opt.Ignore())
                    .ForMember(x => x.Notes, opt => opt.Ignore())
                    .ForMember(x => x.ParentIssue, opt => opt.Ignore())
                    .ForMember(x => x.FixedVersion, opt => opt.Ignore())
                    .ForMember(x => x.IsPrivate, opt => opt.Ignore())
                    .ForMember(x => x.TotalSpentHours, opt => opt.Ignore())
                    .ForMember(x => x.TotalEstimatedHours, opt => opt.Ignore())
                    .ForMember(x => x.Journals, opt => opt.Ignore())
                    .ForMember(x => x.Changesets, opt => opt.Ignore())
                    .ForMember(x => x.Attachments, opt => opt.Ignore())
                    .ForMember(x => x.Relations, opt => opt.Ignore())
                    .ForMember(x => x.Children, opt => opt.Ignore())
                    .ForMember(x => x.Uploads, opt => opt.Ignore())
                    .ForMember(x => x.Watchers, opt => opt.Ignore());

                cfg.CreateMap<IssuePriority, CommonRemineEntities.Priority>();
                cfg.CreateMap<Project, CommonRemineEntities.Project>();
                cfg.CreateMap<IssueStatus, CommonRemineEntities.Status>();
                cfg.CreateMap<Tracker, CommonRemineEntities.Tracker>();
                cfg.CreateMap<ProjectTracker, CommonRemineEntities.Tracker>();

                cfg.CreateMap<User, CommonRemineEntities.User>();
                cfg.CreateMap<IdentifiableName, CommonRemineEntities.User>();

                cfg.CreateMap<CustomFieldValue, CommonRemineEntities.CustomFieldValue>()
                    .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.Info))
                    .ReverseMap();

                cfg.CreateMap<IssueCustomField, CommonRemineEntities.CustomField>()
                    .ConvertUsing<CustomFieldConverterFromRedmine>();

                cfg.CreateMap<CommonRemineEntities.CustomField, IssueCustomField>()
                    .ConvertUsing<CustomFieldConverterToRedmine>();

            });

            return mapConfig.CreateMapper();
        }

        internal class CustomFieldConverterFromRedmine : ITypeConverter<IssueCustomField, CommonRemineEntities.CustomField>
        {
            public CommonRemineEntities.CustomField Convert(
                IssueCustomField source,
                CommonRemineEntities.CustomField destination,
                ResolutionContext context)
            {
                destination = new CommonRemineEntities.CustomField
                {
                    Id = source.Id,
                    Name = source.Name,
                    Values = source.Values
                        .Select(v => context.Mapper.Map<CommonRemineEntities.CustomFieldValue>(v))
                        .ToList()
                };

                return destination;
            }
        }

        internal class CustomFieldConverterToRedmine : ITypeConverter<CommonRemineEntities.CustomField, IssueCustomField>
        {
            public IssueCustomField Convert(
                CommonRemineEntities.CustomField source,
                IssueCustomField destination,
                ResolutionContext context)
            {
                destination = new IssueCustomField
                {
                    Id = source.Id,
                    Name = source.Name,
                    Values = source.Values
                        .Select(v => context.Mapper.Map<CustomFieldValue>(v))
                        .ToList()
                };

                return destination;
            }
        }
    }
}
