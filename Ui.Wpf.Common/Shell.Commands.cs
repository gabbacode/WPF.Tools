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
using System.Windows.Input;
using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Common
{
    public partial class Shell
    {
        [Reactive] public int MenuHeight { get; set; }
        [Reactive] public ReadOnlyObservableCollection<MenuItem> MenuItems { get; set; }

        private SourceList<RootMenu> Roots;
        private SourceList<CommandItem> Commands;

        private IView ActualCommandView;

        public Shell()
        {
            MenuHeight = 0;
            ActualCommandView = null;
            Roots = new SourceList<RootMenu>();
            Commands = new SourceList<CommandItem>();

            Roots
                .Connect()
                .AutoRefresh()
                .Transform(x => x.Root)
                .Bind(out ReadOnlyObservableCollection<MenuItem> temp)
                .Subscribe();

            MenuItems = temp;

            Commands
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

        private RootMenu CheckRoot(string menuName)
        {
            var root = Roots.Items.Where(x => x.Name == menuName).FirstOrDefault();
            if (root != null)
                return root;

            root = new RootMenu { Name = menuName, Root = new MenuItem { Header = menuName } };
            Roots.Add(root);

            root.Observable = Commands
                .Connect()
                .AutoRefresh()
                .Filter(x => x.MenuName == menuName && x.Visible);

            root.Observable
                .WhereReasonsAre(ListChangeReason.Add)
                .Subscribe(x => x
                        .Select(q => q.Item.Current)
                        .ToList()
                        .ForEach(cmd =>
                        {
                            var mi = PrepareMenuItem(cmd);
                            cmd.Item = mi;
                            root.Root.Items.Add(mi);

                            if (cmd.Separator)
                                root.Root.Items.Add(new Separator());
                        }));

            root.Observable
                .WhereReasonsAre(ListChangeReason.Remove)
                .Subscribe(x => x
                        .Select(q => q.Item.Current)
                        .ToList()
                        .ForEach(cmd => root.Root.Items.Remove(cmd.Item)));

            root.Observable
                .WhenAnyPropertyChanged("KeyBind")
                .Subscribe(cmd => RebindKeys(cmd));

            return root;
        }

        private void RebindKeys(CommandItem cmd)
        {
            cmd.KeyBind.Command = cmd.Item.Command;

            var wnd = cmd.Parent.FindParent<MetroWindow>();
            wnd.InputBindings.SkippedAdd(cmd.KeyBind);

            cmd.Item.InputGestureText =
                $"{ModToStr(cmd.KeyBind.Modifiers)}{cmd.KeyBind.Key.ToString()}";
        }

        private MenuItem PrepareMenuItem(CommandItem ci)
        {
            var mi = new MenuItem
            {
                Header = ci.Name,
                DataContext = ci.VM,
                IsChecked = ci.IsChecked
            };

            mi.SetBinding(MenuItem.CommandProperty, new Binding(ci.CmdFunc));

            if (ci.Type == CommandType.VMInstance)
            {
                mi.SetBinding(MenuItem.CommandParameterProperty,
                  new Binding(".")
                  { RelativeSource = new RelativeSource(RelativeSourceMode.Self) });
            }

            return mi;
        }

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

        public CommandItem AddGlobalCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm, bool addSeparator = false)
        {
            var root = CheckRoot(menuName);

            var exsts = Commands.Items
                .Where(x => x.Parent == root.Root && x.Name == cmdName)
                .Count();

            if (exsts > 0)
                throw new Exception("Command already exists");

            CommandItem ci = new CommandItem
            {
                MenuName = menuName,
                VM = vm,
                VMType = vm.GetType(),
                Name = cmdName,
                IsChecked = false,
                Type = CommandType.Global,
                CmdFunc = cmdFunc,
                Parent = root.Root,
                Separator = addSeparator
            };

            Commands.Add(ci);
            return ci;
        }

        public CommandItem AddVMCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm, bool addSeparator = false)
        {
            var root = CheckRoot(menuName);

            var ci = Commands.Items
                .Where(x => x.Parent == root.Root && x.Name == cmdName
                    && x.Type == CommandType.VMType)
                .FirstOrDefault();

            if (ci == null)
            {
                ci = new CommandItem
                {
                    MenuName = menuName,
                    VM = vm,
                    VMType = vm.GetType(),
                    Name = cmdName,
                    IsChecked = false,
                    Type = CommandType.VMType,
                    CmdFunc = cmdFunc,
                    Parent = root.Root,
                    Separator = addSeparator
                };

                Commands.Add(ci);
            }

            return ci;
        }

        public CommandItem AddInstanceCommand(string menuName, string cmdName, string cmdFunc, IViewModel vm)
        {
            var root = CheckRoot(menuName);

            var ci = Commands.Items
                .Where(x => x.Parent == root.Root && x.Name == cmdName
                    && x.VM == vm && x.Type == CommandType.VMInstance)
                .FirstOrDefault();

            if (ci == null)
            {
                ci = new CommandItem
                {
                    MenuName = menuName,
                    VM = vm,
                    VMType = vm.GetType(),
                    Name = cmdName,
                    IsChecked = false,
                    Type = CommandType.VMInstance,
                    CmdFunc = cmdFunc,
                    Parent = root.Root
                };

                Commands.Add(ci);
            }

            return ci;
        }

        private void DeactivateVMCommands()
        {
            if (ActualCommandView == null)
                return;

            var vm = ActualCommandView.ViewModel;

            Commands.Items
                .Where(cmd => cmd.Type == CommandType.VMType && cmd.VMType == vm.GetType())
                .ToList()
                .ForEach(cmd =>
                {
                    cmd.Item.DataContext = null;
                    cmd.Item.IsEnabled = false;

                    var wnd = cmd.Parent.FindParent<MetroWindow>();
                    wnd.InputBindings.MatchedRemove(cmd.KeyBind);
                });

            Commands.Items
                .Where(cmd => cmd.Type == CommandType.VMInstance && cmd.VM == vm)
                .ToList()
                .ForEach(cmd => cmd.Visible = false);
        }

        private void ActivateVMCommands()
        {
            var vm = ActualCommandView.ViewModel;

            Commands.Items
                .Where(cmd => cmd.Type == CommandType.VMType && cmd.VMType == vm.GetType())
                .ToList()
                .ForEach(cmd =>
                {
                    cmd.Item.DataContext = vm;
                    cmd.Item.IsEnabled = true;

                    RebindKeys(cmd);
                });

            Commands.Items
                .Where(cmd => cmd.Type == CommandType.VMInstance && cmd.VM == vm &&
                    !cmd.Parent.Items.Contains(cmd.Item))
                .ToList()
                .ForEach(cmd => cmd.Visible = true);
        }
    }//end of class
}
