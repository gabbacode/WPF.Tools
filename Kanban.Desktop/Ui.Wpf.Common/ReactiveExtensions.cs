using System.Collections.Generic;
using ReactiveUI;

namespace Ui.Wpf.Common
{
    public static class CollectionExtensions
    {
        public static void PublishCollection<T>(
            this ReactiveList<T> collection,
            IEnumerable<T> source)
        {
            using (collection.SuppressChangeNotifications())
            {
                collection.Clear();

                //workaround AddRange bug
                foreach (var item in source)
                {
                    collection.Add(item);
                }
            }
        }
    }
}
