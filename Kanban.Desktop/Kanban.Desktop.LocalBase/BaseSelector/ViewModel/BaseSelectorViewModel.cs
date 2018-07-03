using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Desktop.LocalBase.BaseSelector.ViewModel
{
    class BaseSelectorViewModel : IBaseSelectorViewModel
    {
        public BaseSelectorViewModel()
        {
            Title = "312";
        }

        public string BaseName { get; set; }

        public string Title { get ; set ; }
    }
}
