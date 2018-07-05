using Kanban.Desktop.LocalBase.BaseSelector.ViewModel;
using System;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.BaseSelector.View
{
    /// <summary>
    /// Interaction logic for BaseSelectorView.xaml
    /// </summary>
    public partial class BaseSelectorView : IBaseSelectorView
    {
        public BaseSelectorView(IBaseSelectorViewModel viewModel)
        {
            InitializeComponent();
            ViewModel   = viewModel;
            DataContext = ViewModel;
            if (ViewModel.CloseWindow == null)
                ViewModel.CloseWindow = new Action(Close);
        }

        public IBaseSelectorViewModel ViewModel { get; set; }

        public void Configure(UiShowOptions options)
        {

        }
    }
}
