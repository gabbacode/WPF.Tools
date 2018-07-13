using Kanban.Desktop.LocalBase.DimensionEdit.View;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.DimensionEdit_unused.View
{
    /// <summary>
    /// Interaction logic for DimensionEditView.xaml
    /// </summary>
    public partial class DimensionEditView : IDimensionEditView
    {
        public DimensionEditView()
        {
            InitializeComponent();
        }

        public IViewModel ViewModel { get; set; }
        public void Configure(UiShowOptions options)
        {
        }
    }
}
