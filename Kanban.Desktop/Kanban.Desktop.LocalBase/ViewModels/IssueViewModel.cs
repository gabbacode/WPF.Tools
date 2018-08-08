using System;
using System.Reactive.Linq;
using AutoMapper;
using Data.Entities.Common.LocalBase;
using Kanban.Desktop.LocalBase.Models;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.ViewModels
{
    public class IssueViewRequest : ViewRequest
    {
        public int? IssueId { get; set; }
        public IScopeModel Scope { get; set; }
        public BoardInfo Board { get; set; }
    }

    public class IssueViewModel : ViewModelBase, IViewModel, IInitializableViewModel
    {
        private readonly IMapper mapper;
        private IScopeModel scope;

        public int Id { get; set; }
        public DateTime Created { get; set; }

        public ReactiveList<RowInfo> AwailableRows { get; set; } = new ReactiveList<RowInfo>();
        public ReactiveList<ColumnInfo> AwailableColumns { get; set; } = new ReactiveList<ColumnInfo>();

        [Reactive] public string Head { get; set; }
        [Reactive] public string Body { get; set; }
        [Reactive] public RowInfo Row { get; set; }
        [Reactive] public ColumnInfo Column { get; set; }
        [Reactive] public string Color { get; set; }
        private BoardInfo board;

        public ReactiveCommand CancelCommand { get; set; }
        public ReactiveCommand SaveCommand { get; set; }

        public IssueViewModel()
        {
            mapper = CreateMapper();

            var issueFilled = this.WhenAnyValue(t => t.Head, t => t.Body, t => t.Row, t => t.Column,
                (sh, sb, sr, sc) => sr != null && sc != null &&
                                    !string.IsNullOrEmpty(sh) && !string.IsNullOrEmpty(sb));
            //TODO :add selectcommand when click uneditable with nulling all "selected" fields

            SaveCommand = ReactiveCommand.CreateFromTask(async _ =>
            {
                var editedIssue = new LocalIssue() { Board = board };

                mapper.Map(this, editedIssue);

                await scope.CreateOrUpdateIssueAsync(editedIssue);

                Close();
            }, issueFilled);

            CancelCommand = ReactiveCommand.Create(Close);
        }

        public void Initialize(ViewRequest viewRequest)
        {
            if (viewRequest is IssueViewRequest request)
            {
                scope = request.Scope;

                 board = request.Board;
                var issueId = request.IssueId;
                if (issueId == 0)
                    issueId = null;

                Observable.FromAsync(() => scope.LoadOrCreateIssueAsync(issueId))
                    .ObserveOnDispatcher()
                    .Subscribe(issue =>
                        mapper.Map(issue, this));
            }

            Observable.FromAsync(() => scope.GetRowsAsync(board.Id))
                .ObserveOnDispatcher()
                .Subscribe(rows =>
                {
                    AwailableRows.Clear();
                    AwailableRows.AddRange(rows);
                });

            Observable.FromAsync(() => scope.GetColumnsAsync(board.Id))
                .ObserveOnDispatcher()
                .Subscribe(columns =>
                    AwailableColumns.PublishCollection(columns));

            Title = $"Редактирование задачи {Head}";
        }

        private IMapper CreateMapper()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
                cfg.AddProfile(typeof(MapperProfileSqliteRepos)));

            return mapperConfig.CreateMapper();
        }

        private class MapperProfileSqliteRepos : Profile
        {
            public MapperProfileSqliteRepos()
            {
                CreateMap<LocalIssue, IssueViewModel>();

                CreateMap<IssueViewModel, LocalIssue>();
            }
        }
    }
}
