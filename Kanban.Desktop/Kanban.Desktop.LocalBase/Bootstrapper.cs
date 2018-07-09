using Autofac;
using Data.Sources.LocalStorage.Sqlite;
using Kanban.Desktop.LocalBase.DataBaseSelector.Model;
using Kanban.Desktop.LocalBase.DataBaseSelector.View;
using Kanban.Desktop.LocalBase.DataBaseSelector.ViewModel;
using Kanban.Desktop.LocalBase.LocalBoard.Model;
using Kanban.Desktop.LocalBase.LocalBoard.View;
using Kanban.Desktop.LocalBase.LocalBoard.ViewModel;
using Kanban.Desktop.Settings;
using Ui.Wpf.Common;

namespace Kanban.Desktop.LocalBase
{
    class Bootstrapper : IBootstraper
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

            builder.RegisterType<MainWindow>().As<IDockWindow>();

            builder
               .RegisterType<SqliteLocalRepository>()
               .SingleInstance();

            //TODO Modules discovering
            ConfigureDataBaseSelector(builder);

            ConfigureLocalBoardView(builder);

            ConfigureSettings(builder);

            return builder.Build();
        }

        private static void ConfigureDataBaseSelector(ContainerBuilder builder)
        {
            builder.RegisterType<DataBaseSelectorModel>()
                .As<IDataBaseSelectorModel>();

            builder.RegisterType<BaseSelectorViewModel>()
                .As<IBaseSelectorViewModel>();

            builder.RegisterType<DataBaseSelectorView>()
                .As<IBaseSelectorView>();
        }

        private static void ConfigureLocalBoardView(ContainerBuilder builder)
        {
            builder.RegisterType<LocalBoardModel>()
                .As<ILocalBoardModel>();

            builder.RegisterType<LocalBoardViewModel>()
                .As<ILocalBoardViewModel>();

            builder.RegisterType<LocalBoardView>()
                .As<ILocalBoardView>();
        }

        private static void ConfigureSettings(ContainerBuilder builder)
        {
            builder
                .RegisterType<SettingsViewModel>()
                .As<ISettingsViewModel>();

            builder
                .RegisterType<SettingsView>()
                .As<ISettingsView>();
        }
    }
}
