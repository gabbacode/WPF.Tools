using Autofac;
using Ui.Wpf.Common;
using Ui.Wpf.Demo.ViewModels;
using Ui.Wpf.Demo.Views;

namespace Ui.Wpf.Demo
{
    public class Bootstrap : IBootstraper
    {
        public IShell Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Shell>().As<IShell>().SingleInstance();
            builder.RegisterType<MainWindow>().As<IDockWindow>().SingleInstance();

            builder.RegisterType<ToolsView>();
            builder.RegisterType<ToolsViewModel>();

            builder.RegisterType<MainView>();
            builder.RegisterType<MainViewModel>();

            builder.RegisterType<FlyoutDemoView>();
            builder.RegisterType<FlyoutDemoViewModel>();

            builder.RegisterType<ChildWindowDemoView>();
            builder.RegisterType<ChildWindowDemoViewModel>();

            builder.RegisterType<TextBoxView>();
            builder.RegisterType<TextBoxViewModel>();

            var container = builder.Build();

            var shell = container.Resolve<IShell>();
            shell.Container = container;

            return shell;
        }
    }
}