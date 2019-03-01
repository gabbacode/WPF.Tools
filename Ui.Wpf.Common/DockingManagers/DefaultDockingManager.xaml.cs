using Xceed.Wpf.AvalonDock;

namespace Ui.Wpf.Common.DockingManagers
{
    /// <summary>
    /// Interaction logic for DefaultDockingManager.xaml
    /// </summary>
    public partial class DefaultDockingManager : DockingManager
    {
        public const string Tools = "Tools";
        public const string Views = "Views";

        public DefaultDockingManager()
        {
            InitializeComponent();
        }
    }
}