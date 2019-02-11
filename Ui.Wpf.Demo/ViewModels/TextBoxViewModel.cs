using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
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

    public class TextBoxViewModel : ViewModelBase, IInitializableViewModel, IResultableViewModel<string>
    {
        [Reactive] public string Text { get; set; }

        public ReactiveCommand<Unit, Unit> OkCommand { get; set; }

        public TextBoxViewModel()
        {
            OkCommand = ReactiveCommand.Create(() =>
            {
                ViewResult = Text;
                Close();
            });
        }

        public void Initialize(ViewRequest viewRequest)
        {
            if (viewRequest is TextBoxViewRequest tbRequest)
                Text = tbRequest.Text;
        }

        public string ViewResult { get; set; }
    }
}