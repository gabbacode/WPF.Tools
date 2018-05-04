using AutoMapper;
using Data.Entities.Common.Redmine;
using Kanban.Desktop.Issues.Model;
using System;
using Ui.Wpf.Common;

namespace Kanban.Desktop.Issues.ViewModel
{
    public class IssueViewModel : IIssueViewModel
    {
        public IssueViewModel(IIssueModel model)
        {
            Model = model;

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Issue, IssueViewModel>();
            });

            Mapper = mapConfig.CreateMapper();
        }

        public void Initialize(ViewRequest viewRequest)
        {
            var issueId = (viewRequest as IssueViewRequest)?.IssueId;

            var issue = Model.LoadOrCreate(issueId);

            Mapper.Map(issue, this);

            Title = issue.ToString();
        }

        public string Title { get; set; }

        public int? Id { get; set; }

        public User AssignedTo { get; set; }

        public Project Project { get; set; }

        public Status Status { get; set; }

        public Priority Priority { get; set; }

        public Tracker Tracker { get; set; }

        public string Subject { get; set; }

        public string Description { get; set; }

        public DateTime? CreatedOn { get; set; }

        public IIssueModel Model { get; }

        public IMapper Mapper { get; }
    }
}
