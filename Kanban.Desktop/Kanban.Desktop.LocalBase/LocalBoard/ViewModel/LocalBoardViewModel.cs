using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;
using Kanban.Desktop.LocalBase.LocalBoard.Model;
using ReactiveUI;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.LocalBoard.ViewModel
{
    public class LocalBoardViewModel : ViewModelBase, ILocalBoardViewModel
    {

        private readonly ILocalBoardModel _model;
        public ReactiveList<LocalIssue> Issues { get; set; }
        public ReactiveList<ColumnInfo> Columns { get; set; }
        public ReactiveList<RowInfo> Rows { get; set; }

        public LocalBoardViewModel(ILocalBoardModel model)
        {
            _model = model;
        }

    }
}
