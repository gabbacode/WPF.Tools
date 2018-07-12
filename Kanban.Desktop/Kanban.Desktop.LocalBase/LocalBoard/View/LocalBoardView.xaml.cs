using System.Windows;
using System.Windows.Input;
using Kanban.Desktop.LocalBase.LocalBoard.ViewModel;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Common.ViewModels;
using Ui.Wpf.KanbanControl.Elements.CardElement;

namespace Kanban.Desktop.LocalBase.LocalBoard.View
{
    /// <summary>
    /// Interaction logic for LocalBoardView.xaml
    /// </summary>
    public partial class LocalBoardView : ILocalBoardView
    {
        public LocalBoardView(ILocalBoardViewModel localBoardViewModel)
        {
            InitializeComponent();
            ViewModel = localBoardViewModel;
            DataContext = ViewModel;
        }

        public IViewModel ViewModel { get; set; }

        public void Configure(UiShowOptions options)
        {
            ViewModel.Title = options.Title;
        }

    }
}
