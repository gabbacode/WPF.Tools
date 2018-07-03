using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Autofac;
using Data.Sources.LocalStorage.Sqlite;
using Kanban.Desktop.LocalBase.BaseSelector.View;
using Microsoft.Win32;
using Ookii.Dialogs.Wpf;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ShowOptions;

namespace Kanban.Desktop.LocalBase
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var shell = UiStarter.Start<IBaseSelectorView>(
             new Bootstrapper(),
             new UiShowStartWindowOptions
             {
                 Title = "Kanban.Desktop",
                 ToolPaneWidth = 100
             });

           // var database = shell.Container.Resolve<SqliteLocalRepository>();

           // shell.ShowView<BaseSelectorView>();

            //var tt = new OpenFileDialog() { Filter = "SQLite DataBase | *.db" };
            //var pp = "";
            //if ((bool)tt.ShowDialog())
            //    pp = tt.FileName;
           

            //var ttt = new SaveFileDialog() { Filter = "SQLite DataBase | *.db" };
            //var ppp = "";
            //if ((bool)ttt.ShowDialog())
            //    ppp = ttt.FileName;
            //var cols = database.GetColumns();

            //var t = new VistaFolderBrowserDialog();
            //var p = "";
            //if ((bool)t.ShowDialog())
            //    p = t.SelectedPath;

        }
    }
}
