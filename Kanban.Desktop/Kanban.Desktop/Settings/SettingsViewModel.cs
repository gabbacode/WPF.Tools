using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;

namespace Kanban.Desktop.Settings
{
    public class SettingsViewModel : ISettingsViewModel
    {
        public void Initialize(ViewRequest viewRequest)
        {
        }
        
        [Reactive] public string Title { get; set; }
        
        public bool UseDynamicDimensionts { get; set; }
    }
}