using DynamicData;
using MahApps.Metro.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Common
{
    public class RootMenu : ReactiveObject
    {
        public string Name { get; set; }
        public MenuItem Root { get; set; }

        public IObservable<IChangeSet<CommandItem>> Observable { get; set; }
    }

    public enum CommandType
    {
        // single cmd, always visible
        Global,
        // cmd from selected view-type, disable when other view-type selected
        VMType,
        // single cmd, show only for selected view
        VMInstance
    }

    public class CommandItem : ReactiveObject
    {
        [Reactive] public string MenuName { get; set; }
        [Reactive] public IViewModel VM { get; set; }
        [Reactive] public Type VMType { get; set; }
        [Reactive] public string Name { get; set; }
        [Reactive] public bool IsChecked { get; set; }
        [Reactive] public string CmdFunc { get; set; }
        public CommandType Type { get; set; }
        public MenuItem Item { get; set; }
        public MenuItem Parent { get; set; }
        [Reactive] public KeyBinding KeyBind { get; set; }
        [Reactive] public bool Visible { get; set; }
        [Reactive] public bool Separator { get; set; }

        public CommandItem()
        {
            KeyBind = new KeyBinding();
            Visible = true;

            this.WhenAnyValue(x => x.Name)
                .Where(x => Item != null)
                .Subscribe(x => Item.Header = x);

            this.WhenAnyValue(x => x.IsChecked)
                .Where(x => Item != null)
                .Subscribe(x => Item.IsChecked = x);
        }

        public CommandItem SetHotKey(ModifierKeys mk, Key ky)
        {
            KeyBind = new KeyBinding
            {
                Key = ky,
                Modifiers = mk
            };

            return this;
        }
    }

    public static class CommandHelperExtension
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            var parent = VisualTreeHelper.GetParent(child);
            return parent is T t ? t : FindParent<T>(parent);
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dict, TKey key)
            where TValue : new()
        {
            TValue val;

            if (!dict.TryGetValue(key, out val))
            {
                val = new TValue();
                dict.Add(key, val);
            }

            return val;
        }

        /// <summary>
        /// Skip add if modifiers and key identity
        /// </summary>
        public static void SkippedAdd(this InputBindingCollection coll, KeyBinding kb)
        {
            foreach (KeyBinding elem in coll)
                if (elem.Modifiers == kb.Modifiers && elem.Key == kb.Key)
                    return;

            coll.Add(kb);
        }

        /// <summary>
        /// Remove if modifiers and key matched
        /// </summary>
        public static void MatchedRemove(this InputBindingCollection coll, KeyBinding kb)
        {
            foreach (KeyBinding elem in coll)
                if (elem.Modifiers == kb.Modifiers && elem.Key == kb.Key)
                {
                    coll.Remove(elem);
                    return;
                }
        }
    }
}
