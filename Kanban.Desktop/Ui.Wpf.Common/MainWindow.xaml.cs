using System;

namespace Ui.Wpf.Common
{
    /// <summary>
    /// codebehind for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : IDockWindow
    {
        public MainWindow(IShell shell)
        {
            Shell = shell;
            InitializeComponent();
        }

        private IShell Shell { get; }

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
