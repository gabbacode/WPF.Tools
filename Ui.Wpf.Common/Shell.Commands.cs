using MahApps.Metro.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Common
{
    public partial class Shell
    {
        [Reactive] public int MenuHeight { get; set; }

        public ReactiveList<MenuItem> MenuItems { get; set; } = new ReactiveList<MenuItem>();

        private List<CommandItem> GlobalCommandItems = new List<CommandItem>();
        private Dictionary<Type, List<CommandItem>> VMTypeCommandItems = new Dictionary<Type, List<CommandItem>>();

        private IView ActualCommandView;

        public Shell()
        {
            ActualCommandView = null;

            MenuItems.Changed.Subscribe(_ => MenuHeight = MenuItems.Count > 0 ? 30 : 0);

            this.ObservableForProperty(w => w.SelectedView)
                .Subscribe(v =>
                {
                    DeactivateVMCommands();
                    ActualCommandView = v.Value;
                    ActivateVMCommands();
                });
        }

        public CommandItem AddGlobalCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm)
        {
            var m = GetMenu(menuName);

            var exsts = GlobalCommandItems
                .Where(x => x.Parent == m && (string)x.Item.Header == cmdName)
                .Count();

            if (exsts > 0)
                throw new Exception("Command already exists");

            var c = new MenuItem { Header = cmdName, DataContext = vm };
            c.SetBinding(MenuItem.CommandProperty, new Binding(cmdFunc));
            m.Items.Add(c);

            CommandItem ci = new CommandItem
            {
                Type = CommandType.Global,
                Item = c,
                Parent = m
            };

            GlobalCommandItems.Add(ci);

            return ci;
        }

        public CommandItem AddVMTypeCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm)
        {
            var m = GetMenu(menuName);

            var cmdList = VMTypeCommandItems.GetOrCreate(vm.GetType());

            var ci = cmdList
                .Where(x => x.Parent == m && (string)x.Item.Header == cmdName)
                .FirstOrDefault();

            if (ci == null)
            {
                var c = new MenuItem { Header = cmdName, DataContext = vm };
                c.SetBinding(MenuItem.CommandProperty, new Binding(cmdFunc));
                m.Items.Add(c);

                ci = new CommandItem
                {
                    Type = CommandType.VMType,
                    Item = c,
                    Parent = m
                };

                cmdList.Add(ci);
            }

            return ci;
        }

        private MenuItem GetMenu(string menuName)
        {
            var m = MenuItems
                .Where(x => (string)x.Header == menuName)
                .FirstOrDefault();

            if (m == null)
            {
                m = new MenuItem { Header = menuName };
                MenuItems.Add(m);
            }

            return m;
        }

        private void DeactivateVMCommands()
        {
            if (ActualCommandView == null)
                return;

            var vm = ActualCommandView.ViewModel;
            var vmTyp = vm.GetType();
            var cmdList = VMTypeCommandItems.GetOrCreate(vmTyp);
            foreach (var ci in cmdList)
            {
                ci.Item.DataContext = null;
                ci.Item.IsEnabled = false;

                if (ci.KeyBind != null)
                {
                    var wnd = ci.Parent.FindParent<MetroWindow>();
                    wnd.InputBindings.MatchedRemove(ci.KeyBind);
                }
            }
        }

        private void ActivateVMCommands()
        {
            var vm = ActualCommandView.ViewModel;
            var vmTyp = vm.GetType();
            var cmdList = VMTypeCommandItems.GetOrCreate(vmTyp);
            foreach (var ci in cmdList)
            {
                ci.Item.DataContext = vm;
                ci.Item.IsEnabled = true;

                if (ci.KeyBind != null)
                {
                    ci.KeyBind.Command = ci.Item.Command;

                    var wnd = ci.Parent.FindParent<MetroWindow>();
                    wnd.InputBindings.SkippedAdd(ci.KeyBind);
                }
            }
        }
    }//end of class
}
