using System;
using MahApps.Metro.Controls;

namespace Ui.Wpf.Common
{
    /// <summary>
    /// codebehind for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow, IDockWindow
    {
        public MainWindow(Shell shell)
        {
            Shell = shell;
            InitializeComponent();
        }

        public Shell Shell { get; }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            //TODO do with behavior
            Shell.AttachDockingManager(DockingManager);

            DataContext = Shell;
        }

        private void ActiveContentChanged(object sender, EventArgs e)
        {

        }
    }
}
