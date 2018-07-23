using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ui.Wpf.Common;
using Autofac;

namespace Kanban.Desktop.LocalBase.Models
{
    public interface IAppModel
    {
        IEnumerable<string> GetRecentDocuments();
        void AddRecent(string doc);
        void RemoveRecent(string doc);

        string Caption { get; set; }

        void LoadConfig();
        void SaveConfig();

        IScopeModel CreateScope(string uri);
        IScopeModel LoadScope(string uri);
    }

    public class AppConfig
    {
        public List<string> Recent { get; set; }
        public string Caption { get; set; }

        public AppConfig()
        {
            Recent = new List<string>();
            Caption = "";
        }
    }

    public class AppModel : IAppModel
    {
        private readonly IShell shell_;

        private AppConfig appConfig_;
        private string path_;

        public string Caption
        {
            get { return appConfig_.Caption; }
            set { appConfig_.Caption = value; }
        }

        public AppModel(IShell shell)
        {
            shell_ = shell;

            path_ = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData).ToString();
            path_ += "\\kanban.config";

            appConfig_ = new AppConfig();
        }

        public void AddRecent(string doc)
        {
            appConfig_.Recent.Insert(0, doc);
        }

        public IEnumerable<string> GetRecentDocuments()
        {
            return appConfig_.Recent;
        }

        public void RemoveRecent(string doc)
        {
            appConfig_.Recent.Remove(doc);
        }

        public void LoadConfig()
        {
            if (File.Exists(path_))
            {
                string data = File.ReadAllText(path_);
                appConfig_ = JsonConvert.DeserializeObject<AppConfig>(data);
            }
        }

        public void SaveConfig()
        {
            string data = JsonConvert.SerializeObject(appConfig_);
            File.WriteAllText(path_, data);
        }

        public IScopeModel CreateScope(string uri)
        {
            var scope = shell_
                .Container
                .Resolve<IScopeModel>(new NamedParameter("uri", uri));

            return scope;
        }

        public IScopeModel LoadScope(string uri)
        {
            return null;
        }
    }
}
