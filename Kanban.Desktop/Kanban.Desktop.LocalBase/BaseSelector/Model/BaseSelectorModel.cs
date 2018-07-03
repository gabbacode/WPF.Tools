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
    }
}
