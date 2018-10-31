using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ui.Wpf.Common
{
    public static class DynamicHelper
    {
        public static void Delete<T>(this SourceList<T> list, Func<T, bool> predicate)
        {
            var toRemove = list.Items.Where(predicate);
            list.RemoveMany(toRemove);
        }

        /// <summary>
        /// Spawn observable r/o collection for UI use
        /// </summary>
        public static ReadOnlyObservableCollection<T> SpawnCollection<T>(this SourceList<T> list)
        {
            list
                .Connect()
                .Bind(out ReadOnlyObservableCollection<T> temp)
                .Subscribe();

            return temp;
        }

        public static T First<T>(this SourceList<T> list, Func<T, bool> predicate = null)
        {
            return predicate == null ? list.Items.First() : list.Items.First(predicate);
        }

        public static void ClearAndAddRange<T>(this SourceList<T> list, IEnumerable<T> data)
        {
            list.Edit(il =>
            {
                il.Clear();
                il.AddRange(data);
            });
        }
    }//end of class
}
