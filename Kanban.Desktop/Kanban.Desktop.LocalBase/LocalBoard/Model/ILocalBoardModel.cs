using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Entities.Common.LocalBase;

namespace Kanban.Desktop.LocalBase.LocalBoard.Model
{
    interface ILocalBoardModel
    {
        bool SaveIssueState(LocalIssue issue);
        bool SaveOrUpdateColumn(ColumnInfo column);
        bool SaveOrUpdateRow(RowInfo row);
    }
}
