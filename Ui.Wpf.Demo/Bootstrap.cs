using Autofac;
using Ui.Wpf.Common;

namespace Ui.Wpf.Demo
{
    public class Bootstrap : IBootstraper
    {
        public IShell Init()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Shell>().As<IShell>().SingleInstance();
            builder.RegisterType<MainWindow>().As<IDockWindow>().SingleInstance();

            var container = builder.Build();

            var shell = container.Resolve<IShell>();
            shell.Container = container;

            return shell;
        }
    }
}
