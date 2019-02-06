using ReactiveUI.Fody.Helpers;
using Ui.Wpf.Common;
using Ui.Wpf.Common.ViewModels;

namespace Ui.Wpf.Demo.ViewModels
{
    internal class TextBoxViewRequest : ViewRequest
    {
        public string Text { get; set; }

        public TextBoxViewRequest(string text = null)
        {
            Text = text;
        }
    }

    public class TextBoxViewModel : ViewModelBase, IInitializableViewModel
    {
        [Reactive] public string Text { get; set; }

        public void Initialize(ViewRequest viewRequest)
        {
            if (viewRequest is TextBoxViewRequest tbRequest)
                Text = tbRequest.Text;
        }
    }
}