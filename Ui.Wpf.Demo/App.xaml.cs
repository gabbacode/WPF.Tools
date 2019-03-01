using System.Windows;
using Ui.Wpf.Common;
using Ui.Wpf.Common.DockingManagers;
using Ui.Wpf.Common.ShowOptions;
using Ui.Wpf.Demo.Views;

namespace Ui.Wpf.Demo
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = UiStarter.Start<IDockWindow>(
                new Bootstrap(),
                new UiShowStartWindowOptions
                {
                    Title = "WPF.Tools.Demo",
                    DockingManager = new VsDockingManager() // new CustomDockingManager()
                }
            );

            var tools = new[] {VsDockingManager.ToolsLeft, VsDockingManager.ToolsRight, VsDockingManager.ToolsBottom};
            foreach (var tool in tools)
                shell.ShowToolIn<ToolsView>(
                    tool,
                    new ViewRequest(tool),
                    new UiShowOptions {Title = tool}
                );
        }
    }
}