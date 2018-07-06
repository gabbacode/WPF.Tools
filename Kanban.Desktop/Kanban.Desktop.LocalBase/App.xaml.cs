using System.Windows;
using Kanban.Desktop.LocalBase.BaseSelector.View;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;

namespace Kanban.Desktop.LocalBase
{
    public partial class App
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = UiStarter.Start<IDockWindow>(
                new Bootstrapper(),
                new UiShowStartWindowOptions
                {
                    Title         = "Kanban.Desktop.LocalBase",
                    ToolPaneWidth = 100
                });

            shell.ShowView<IBaseSelectorView>(options: new UiShowOptions() {Title = "BaseChoooose"});
        }
    }
}
