using System.Collections.Generic;

namespace Kanban.Desktop.LocalBase.BaseSelector.Model
{
    public interface IBaseSelectorModel
    {
        string CreateDatabase();
        string OpenDatabase();
        void ShiftOrCreateBaseList(string newBaseAddr);
        List<string> GetBaseList();
        bool CheckRecentBase(string basePath);
    }
}
