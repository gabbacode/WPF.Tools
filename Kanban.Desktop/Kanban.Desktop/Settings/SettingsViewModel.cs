using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common.ViewModels;

namespace Kanban.Desktop.Settings
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public void Initialize()
        {
        }
        
        [Reactive] public string Title { get; set; }
        
        public bool UseDynamicDimensionts { get; set; }
    }
}