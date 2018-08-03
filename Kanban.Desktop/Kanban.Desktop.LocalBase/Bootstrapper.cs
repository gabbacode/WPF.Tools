using Autofac;
using Data.Sources.LocalStorage.Sqlite;
using Kanban.Desktop.LocalBase.Models;
using Kanban.Desktop.LocalBase.Views;
using Kanban.Desktop.LocalBase.ViewModels;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

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

            builder
                .RegisterType<ScopeModel>()
                .As<IScopeModel>();

            builder
                .RegisterType<AppModel>()
                .As<IAppModel>()
                .SingleInstance();

            //TODO: Modules discovering?
            ConfigureView<StartupViewModel, StartupView>(builder);
            ConfigureView<WizardViewModel, WizardView>(builder);
            ConfigureView<BoardViewModel, BoardView>(builder);
            ConfigureView<IssueViewModel, IssueView>(builder);

            return builder.Build();
        }

        private static void ConfigureView<TViewModel, TView>(ContainerBuilder builder)
            where TViewModel : IViewModel
            where TView : IView
        {
            builder.RegisterType<TViewModel>();
            builder.RegisterType<TView>();
        }
    }
}
// loadscope,validation,