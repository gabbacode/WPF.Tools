using Xceed.Wpf.AvalonDock;

namespace Ui.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for CustomDockingManager.xaml
    /// </summary>
    public partial class CustomDockingManager : DockingManager
    {
        public const string ToolsLeft = "ToolsLeft";
        public const string ToolsRight = "ToolsRight";
        public const string ToolsBottom = "ToolsBottom";
        public const string Views = "Views";

        public CustomDockingManager()
        {
            InitializeComponent();
        }
    }
}