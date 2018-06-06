using AutoMapper;
using Data.Entities.Common.Redmine;
using Kanban.Desktop.Issues.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using Ui.Wpf.Common;

namespace Kanban.Desktop.Issues.ViewModel
{
    public class IssueViewModel : ReactiveObject, IIssueViewModel
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

            Observable.FromAsync(() => Model.LoadOrCreateAsync(issueId))
                .ObserveOnDispatcher()
                .Subscribe((issue) =>
                {
                    Mapper.Map(issue, this);

                    Title = issue.ToString();
                });
        }

        [Reactive]
        public string Title { get; set; }

        [Reactive]
        public int? Id { get; set; }

        [Reactive]
        public User AssignedTo { get; set; }

        [Reactive]
        public Project Project { get; set; }

        [Reactive]
        public Status Status { get; set; }

        [Reactive]
        public Priority Priority { get; set; }

        [Reactive]
        public Tracker Tracker { get; set; }

        [Reactive]
        public string Subject { get; set; }

        [Reactive]
        public string Description { get; set; }

        [Reactive]
        public DateTime? CreatedOn { get; set; }

        [Reactive]
        public IList<CustomField> CustomFields { get; set; }

        public IIssueModel Model { get; }

        public IMapper Mapper { get; }
    }
}
