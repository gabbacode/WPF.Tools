using AutoMapper;
using Data.Entities.Common.Redmine;
using Kanban.Desktop.Issues.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Reactive.Linq;
using Ui.Wpf.Common;
using Data.Sources.Common.Redmine;
using System.Windows.Input;
using System.Diagnostics;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.Issues.ViewModel
{
    public class IssueViewModel : ViewModelBase, IIssueViewModel
    {
        public IssueViewModel(
            IIssueModel model,
            IRepository redmine)
        {
            Model = model;

            var mapConfig = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Issue, IssueViewModel>();
            });

            Mapper = mapConfig.CreateMapper();

            SaveCommand = ReactiveCommand.CreateFromTask(async (x) =>
            {
                try
                {
                    var newIssue = new Issue
                    {
                        Subject = Subject,
                        Description = Description,
                        AssignedTo = AssignedTo,
                        Project = Project,
                        Status = Status,
                        Tracker = Tracker,
                        Priority = Priority,
                        CustomFields = CustomFields
                    };

                    var saved = await redmine.CreateOrUpdateIssueAsync(newIssue);
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex);
                }
            });

            CancelCommand = ReactiveCommand.Create(() => Close());
        }

        private string GetCustomValue(int customValueId, IList<CustomField> customFields)
        {
            return CustomFields
                .FirstOrDefault(c => c.Id == customValueId)?.Values
                .FirstOrDefault()?.Value;
        }

        private CustomField MakeCustomValue(int fieldTypeId, string value)
        {
            return new CustomField
            {
                Id = fieldTypeId,
                Values = new[] { new CustomFieldValue { Value = value } }
            };
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

        [Reactive]
        public IList<Journal> Journals { get; set; }

        [Reactive]
        public ICommand SaveCommand { get; set; }

        [Reactive]
        public ICommand CancelCommand { get; set; }

        public IIssueModel Model { get; }

        public IMapper Mapper { get; }

    }
}
