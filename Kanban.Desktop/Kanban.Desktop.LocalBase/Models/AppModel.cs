using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kanban.Desktop.LocalBase.Models
{
    public interface IAppModel
    {
        IEnumerable<string> GetRecentDocuments();
        void AddRecent(string doc);
        void RemoveRecent(string doc);

        string Caption { get; set; }

        void Load();
        void Save();
    }

    public class AppData
    {
        public List<string> Recent { get; set; }
        public string Caption { get; set; }

        public AppData()
        {
            Recent = new List<string>();
            Caption = "";
        }
    }

    public class AppModel : IAppModel
    {
        private AppData appData_;

        private string path_;

        public string Caption
        {
            get { return appData_.Caption; }
            set { appData_.Caption = value; }
        }

        public AppModel()
        {
            path_ = Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData).ToString();
            path_ += "\\kanban.config";

            appData_ = new AppData();
        }

        public void AddRecent(string doc)
        {
            appData_.Recent.Insert(0, doc);
        }

        public IEnumerable<string> GetRecentDocuments()
        {
            return appData_.Recent;
        }

        public void RemoveRecent(string doc)
        {
            appData_.Recent.Remove(doc);
        }

        public void Load()
        {
            if (File.Exists(path_))
            {
                string data = File.ReadAllText(path_);
                appData_ = JsonConvert.DeserializeObject<AppData>(data);
            }
        }

        public void Save()
        {
            string data = JsonConvert.SerializeObject(appData_);
            File.WriteAllText(path_, data);
        }
    }
}
