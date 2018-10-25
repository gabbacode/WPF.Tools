using DynamicData;
using MahApps.Metro.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Common
{
    public partial class Shell
    {
        [Reactive] public int MenuHeight { get; set; }

        public SourceList<MenuItem> InternalMenuItems { get; set; } = new SourceList<MenuItem>();

        [Reactive] public ReadOnlyObservableCollection<MenuItem> MenuItems { get; set; }

        private List<CommandItem> GlobalCommandItems;
        private Dictionary<Type, List<CommandItem>> VMCommandItems;
        private Dictionary<IViewModel, List<CommandItem>> InstanceCommandItems;

        private IView ActualCommandView;

        public Shell()
        {
            MenuHeight = 0;
            ActualCommandView = null;
            GlobalCommandItems = new List<CommandItem>();
            VMCommandItems = new Dictionary<Type, List<CommandItem>>();
            InstanceCommandItems = new Dictionary<IViewModel, List<CommandItem>>();

            ReadOnlyObservableCollection<MenuItem> coll;
            InternalMenuItems
                        .Connect()
                        .Bind(out coll)
                        .Subscribe();

            MenuItems = coll;

            InternalMenuItems
                .CountChanged
                .Subscribe(_ => MenuHeight = MenuItems.Count > 0 ? 30 : 0);

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
                Name = cmdName,
                Type = CommandType.Global,
                Item = c,
                Parent = m
            };

            GlobalCommandItems.Add(ci);

            return ci;
        }

        public CommandItem AddVMCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm)
        {
            var m = GetMenu(menuName);
            var cmdList = VMCommandItems.GetOrCreate(vm.GetType());

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
                    Name = cmdName,
                    Type = CommandType.VMType,
                    Item = c,
                    Parent = m
                };

                cmdList.Add(ci);
            }

            return ci;
        }

        public CommandItem AddInstanceCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm)
        {
            var m = GetMenu(menuName);
            var cmdList = InstanceCommandItems.GetOrCreate(vm);

            var ci = cmdList
                .Where(x => x.Parent == m && (string)x.Item.Header == cmdName)
                .FirstOrDefault();

            if (ci == null)
            {
                var c = new MenuItem { Header = cmdName, DataContext = vm };
                c.SetBinding(MenuItem.CommandProperty, new Binding(cmdFunc));

                c.SetBinding(MenuItem.CommandParameterProperty,
                    new Binding(".")
                    { RelativeSource = new RelativeSource(RelativeSourceMode.Self) });

                m.Items.Add(c);

                ci = new CommandItem
                {
                    Name = cmdName,
                    Type = CommandType.VMInstance,
                    Item = c,
                    Parent = m
                };

                cmdList.Add(ci);
            }

            return ci;
        }

        private MenuItem GetMenu(string menuName)
        {
            var m = InternalMenuItems.Items
                .Where(x => (string)x.Header == menuName)
                .FirstOrDefault();

            if (m == null)
            {
                m = new MenuItem { Header = menuName };
                InternalMenuItems.Add(m);
            }

            return m;
        }

        private void DeactivateVMCommands()
        {
            if (ActualCommandView == null)
                return;

            var vm = ActualCommandView.ViewModel;
            var vmTyp = vm.GetType();
            var cmdList = VMCommandItems.GetOrCreate(vmTyp);
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

            cmdList = InstanceCommandItems.GetOrCreate(vm);
            foreach (var ci in cmdList)
                ci.Parent.Items.Remove(ci.Item);
        }

        private void ActivateVMCommands()
        {
            var vm = ActualCommandView.ViewModel;
            var vmTyp = vm.GetType();
            var cmdList = VMCommandItems.GetOrCreate(vmTyp);
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

            cmdList = InstanceCommandItems.GetOrCreate(vm);
            foreach (var ci in cmdList)
                ci.Parent.Items.Add(ci.Item);
        }
    }//end of class
}
