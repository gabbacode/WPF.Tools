using System.Collections.Generic;

namespace Kanban.Desktop.LocalBase.BaseSelector.Model
{
    public interface IBaseSelectorModel
    {
        List<string> GetExistingBases();
    }
}
