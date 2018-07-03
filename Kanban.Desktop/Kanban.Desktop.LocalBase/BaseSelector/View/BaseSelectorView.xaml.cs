using Kanban.Desktop.LocalBase.BaseSelector.ViewModel;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.LocalBase.BaseSelector.View
{
    /// <summary>
    /// Interaction logic for BaseSelectorView.xaml
    /// </summary>
    public partial class BaseSelectorView :  IBaseSelectorView
    {
        public BaseSelectorView(IBaseSelectorViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        public IViewModel ViewModel { get ; set ; }

        public void Configure(UiShowOptions options)
        {
            
        }
    }
}
