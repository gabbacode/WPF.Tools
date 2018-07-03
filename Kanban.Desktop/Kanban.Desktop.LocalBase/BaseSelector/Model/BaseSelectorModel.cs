using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Kanban.Desktop.LocalBase.BaseSelector.Model
{
    public class BaseSelectorModel : IBaseSelectorModel
    {
        private readonly List<string> _existingBases;

        public BaseSelectorModel()
        {
            _existingBases = new List<string>();
        }

        public List<string> GetExistingBases()
        {
            lock (this)
            {
                _existingBases.Clear();
                var basesInFolder = Directory
                  .GetFiles(Directory.GetCurrentDirectory(), "*.db", SearchOption.TopDirectoryOnly)
                  .Select(file => Path.GetFileNameWithoutExtension(file))
                  .ToList();
                foreach (var baseName in basesInFolder)
                    _existingBases.Add(baseName);
            }

            return _existingBases;
        }

        //public async static void GetDbConnStr()
        //{
        //    var mainview = Application.Current.MainWindow as MetroWindow;

        //    var f = Directory.GetCurrentDirectory();
        //    var t = Directory.GetFiles(f, "*.db", SearchOption.TopDirectoryOnly)
        //        .Select(file => Path.GetFileName(file)).ToArray();
        //    var dict = new ResourceDictionary();

        //    for (int i = 0; i < t.Length; i++) dict.Add(i, t[i]);

        //    var dialSetts = new MetroDialogSettings
        //    {
        //        AffirmativeButtonText = "Выбор",
        //        NegativeButtonText="Отмена",
        //        DefaultText=t[0],
        //        CustomResourceDictionary= dict
        //    };

        //    var dbname= await mainview.ShowInputAsync("Выбор локальной базы данных", "Введите название базы", dialSetts);
        //    var db = new SqliteLocalRepository(dbname);
        //    var nmc = new NameValueCollection();
        //    var it = db.GetIssues(nmc);
        //}
    }
}
