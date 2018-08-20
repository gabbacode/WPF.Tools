namespace Ui.Wpf.Common
{
    public class ViewRequest
    {
        public ViewRequest()
        {
        }
        public ViewRequest(string viewId)
        {
            ViewId = viewId;
        }

        public string ViewId
        {
            get;
            set;
        }
    
    }
}