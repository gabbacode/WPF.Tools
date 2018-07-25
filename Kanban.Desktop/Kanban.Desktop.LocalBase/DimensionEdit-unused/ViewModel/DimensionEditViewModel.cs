using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.DimensionEdit.ViewModel
{
    class DimensionEditViewModel : IDimensionEditViewModel, IInitializableViewModel
    {
        public string Title { get; set; }

        public void Initialize(ViewRequest viewRequest)
        {
        }
    }
}
