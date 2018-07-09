using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using Kanban.Desktop.LocalBase.LocalBoard.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Dimensions;

namespace Kanban.Desktop.LocalBase.LocalBoard.ViewModel
{
    public class LocalBoardViewModel : ViewModelBase, ILocalBoardViewModel
    {

        private readonly ILocalBoardModel _model;

        [Reactive] public IDimension VerticalDimension { get; internal set; }

        [Reactive] public IDimension HorizontalDimension { get; internal set; }

        public ReactiveList<LocalIssue> Issues { get; internal set; }

        public LocalBoardViewModel(ILocalBoardModel model)
        {
            _model = model;
            VerticalDimension = _model.GetRows();
            HorizontalDimension = _model.GetColumns();
            Issues=new ReactiveList<LocalIssue>();
            Issues.AddRange(_model.GetIssues());
        }

    }
}
