using System;
using Autofac;
using Data.Sources.Common;
using Data.Sources.Common.Redmine;
using Data.Sources.Redmine;
using Kanban.Desktop.Issues;
using Kanban.Desktop.Issues.Model;
using Kanban.Desktop.Issues.View;
using Kanban.Desktop.Issues.ViewModel;
using Kanban.Desktop.KanbanBoard.Model;
using Kanban.Desktop.KanbanBoard.View;
using Kanban.Desktop.KanbanBoard.ViewModel;
using Kanban.Desktop.Settings;
using Ui.Wpf.Common;

namespace Kanban.Desktop
{
    public class Bootstrap : IBootstraper
    {
        public Shell Init()
        {
            var container = ConfigureContainer();
            var shell = container.Resolve<Shell>();

            shell.Container = container;

            return shell;            
        }

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            builder.RegisterType<Shell>().SingleInstance();

            builder
                .RegisterType<RedmineAutentificationContext>()
                .As<IAutentificationContext>()
                .SingleInstance();

            builder.RegisterType<MainWindow>().As<IDockWindow>();

            //TODO Modules discovering
            ConfigureIssues(builder);

            ConfigureKanbanBoard(builder);

            ConfigureSettings(builder);

            ConfigureRedmine(builder);

            return builder.Build();
        }

        private static void ConfigureIssues(ContainerBuilder builder)
        {
            builder
                .RegisterType<IssuesToolView>()
                .As<IIssuesTool>();

            builder
                .RegisterType<IssueView>()
                .As<IIssueView>();

            builder
                .RegisterType<IssueViewModel>()
                .As<IIssueViewModel>();

            builder
                .RegisterType<IssueModel>()
                .As<IIssueModel>();
        }

        private static void ConfigureKanbanBoard(ContainerBuilder builder)
        {
            builder
                .RegisterType<KanbanBoardModel>()
                .As<IKanbanBoardModel>();
            
            builder
                .RegisterType<KanbanBoardViewModel>()
                .As<IKanbanBoardViewModel>();

            builder
                .RegisterType<KanbanBoardView>()
                .As<IKanbanBoardView>();


            builder
                .RegisterType<KanbanConfigurationRepository>()
                .As<IKanbanConfigurationRepository>();
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

        private static void ConfigureRedmine(ContainerBuilder builder)
        {
            builder
                .RegisterType<RedmineRepository>()
                .As<IRedmineRepository>()
                .SingleInstance();
        }
    }
}
