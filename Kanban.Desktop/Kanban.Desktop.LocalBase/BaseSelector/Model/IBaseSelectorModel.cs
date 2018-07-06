using System.Collections.Generic;

namespace Kanban.Desktop.LocalBase.BaseSelector.Model
{
    public interface IBaseSelectorModel
    {
        string CreateDatabase();
        string OpenDatabase();
        List<string> GetBaseList();
        bool CheckRecentBase(string basePath);
        void ShowSelectedBaseTab(string path);
    }
}
