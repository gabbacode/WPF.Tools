using Autofac;
using Data.Sources.LocalStorage.Sqlite;
using Kanban.Desktop.LocalBase.Models;
using Kanban.Desktop.LocalBase.Views;
using Kanban.Desktop.LocalBase.ViewModels;
using Ui.Wpf.Common;

namespace Kanban.Desktop.LocalBase
{
    public class Bootstrapper : IBootstraper
    {
        public IShell Init()
        {
            var container = ConfigureContainer();
            var shell = container.Resolve<IShell>();
            shell.Container = container;
            return shell;
        }

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Shell>()
                .As<IShell>()
                .SingleInstance();

            builder
                .RegisterType<MainWindow>()
                .As<IDockWindow>();

            builder
                .RegisterType<SqliteLocalRepository>();

            builder.RegisterType<SqliteSettings>()
                .As<IDataBaseSettings>()
                .SingleInstance();

            //TODO: Modules discovering?
            ConfigureView<StartupModel, StartupViewModel, StartupView>(builder);
            ConfigureView<BoardModel, BoardViewModel, BoardView>(builder);
            ConfigureView<IssueModel, IssueViewModel, IssueView>(builder);

            return builder.Build();
        }

        private static void ConfigureView<TModel, TViewModel, TView>(ContainerBuilder builder)
        {
            builder.RegisterType<TModel>();
            builder.RegisterType<TViewModel>();
            builder.RegisterType<TView>();
        }
    }
}
