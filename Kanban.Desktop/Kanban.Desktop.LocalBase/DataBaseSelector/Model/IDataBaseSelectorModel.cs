using System.Collections.Generic;

namespace Kanban.Desktop.LocalBase.DataBaseSelector.Model
{
    public interface IDataBaseSelectorModel
    {
        string CreateDatabase();
        string OpenDatabase();
        List<string> GetBaseList();
        bool CheckDataBaseExists(string basePath);
        void ShowSelectedBaseTab(string path);
    }
}
