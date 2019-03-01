using Xceed.Wpf.AvalonDock;

namespace Ui.Wpf.Common.DockingManagers
{
    /// <summary>
    /// Interaction logic for VsDockingManager.xaml
    /// </summary>
    public partial class VsDockingManager : DockingManager
    {
        public const string ToolsLeft = "ToolsLeft";
        public const string ToolsRight = "ToolsRight";
        public const string ToolsBottom = "ToolsBottom";
        public const string Views = "Views";

        public VsDockingManager()
        {
            InitializeComponent();
        }
    }
}