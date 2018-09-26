using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Common
{
    public enum CommandType
    {
        // single cmd, always visible
        Global,
        // cmd from selected view-type, disable when other view-type selected
        VMType,
        // TODO: single cmd, show only for selected view
        Instance
    }

    public class CommandItem
    {
        public CommandType Type { get; set; }
        public MenuItem Item { get; set; }
        public MenuItem Parent { get; set; }
        public KeyBinding KeyBind { get; set; }

        private string ModToStr(ModifierKeys mk)
        {
            switch (mk)
            {
                case ModifierKeys.None:
                    return "";
                case ModifierKeys.Alt:
                    return "Alt+";
                case ModifierKeys.Control:
                    return "Ctrl+";
                case ModifierKeys.Shift:
                    return "Shift";
                case ModifierKeys.Control | ModifierKeys.Shift:
                    return "Ctrl+Shift+";
                case ModifierKeys.Alt | ModifierKeys.Shift:
                    return "Shift+Alt+";
                case ModifierKeys.Windows:
                    return "Wnd+";
                default:
                    throw new NotImplementedException();
            }
        }

        public CommandItem SetHotKey(ModifierKeys mk, Key ky)
        {
            KeyBind = new KeyBinding
            {
                Key = ky,
                Modifiers = mk,
                Command = Item.Command
            };

            var wnd = Parent.FindParent<MetroWindow>();
            wnd.InputBindings.SkippedAdd(KeyBind);

            Item.InputGestureText = $"{ModToStr(mk)}{ky.ToString()}";

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
