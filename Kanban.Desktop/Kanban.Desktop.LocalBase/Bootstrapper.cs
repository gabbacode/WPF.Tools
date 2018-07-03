using Autofac;
using Data.Sources.Common.Redmine;
using Data.Sources.LocalStorage.Sqlite;
using Kanban.Desktop.LocalBase.BaseSelector;
using Kanban.Desktop.LocalBase.BaseSelector.View;
using Kanban.Desktop.LocalBase.BaseSelector.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            builder.RegisterType<BaseSelectorView>()
                .As<IBaseSelectorView>();

            builder
               .RegisterType<SqliteLocalRepository>()
               .SingleInstance();

            builder.RegisterType<BaseSelectorViewModel>()
                .As<IBaseSelectorViewModel>();

            //TODO Modules discovering
            ConfigureIssues(builder);

            ConfigureKanbanBoard(builder);

            ConfigureSettings(builder);

            return builder.Build();
        }

        private static void ConfigureIssues(ContainerBuilder builder)
        {
            //builder
            //    .RegisterType<IssuesToolView>()
            //    .As<IIssuesTool>();

            //builder
            //    .RegisterType<IssueView>()
            //    .As<IIssueView>();

            //builder
            //    .RegisterType<IssueViewModel>()
            //    .As<IIssueViewModel>();

            //builder
            //    .RegisterType<IssueModel>()
            //    .As<IIssueModel>();
        }

        private static void ConfigureKanbanBoard(ContainerBuilder builder)
        {
            //builder
            //    .RegisterType<KanbanBoardModel>()
            //    .As<IKanbanBoardModel>();

            //builder
            //    .RegisterType<KanbanBoardViewModel>()
            //    .As<IKanbanBoardViewModel>();

            //builder
            //    .RegisterType<KanbanBoardView>()
            //    .As<IKanbanBoardView>();


            //builder
            //    .RegisterType<KanbanConfigurationRepository>()
            //    .As<IKanbanConfigurationRepository>();
        }

        private static void ConfigureSettings(ContainerBuilder builder)
        {
            //builder
            //    .RegisterType<SettingsViewModel>()
            //    .As<ISettingsViewModel>();

            //builder
            //    .RegisterType<SettingsView>()
            //    .As<ISettingsView>();
        }
    }
}
