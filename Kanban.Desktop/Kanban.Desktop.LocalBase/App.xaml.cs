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
             new Bootstrapper());

             var database = shell.Container.Resolve<SqliteLocalRepository>();

            // shell.ShowView<BaseSelectorView>();
            //database.BaseConnstr = $"Data Source = {pp}";
          //  var cols = database.GetColumns();

            //var t = new VistaFolderBrowserDialog();
            //var p = "";
            //if ((bool)t.ShowDialog())
            //    p = t.SelectedPath;

        }
    }
}
