using System;

namespace Ui.Wpf.KanbanControl.Elements
{
    public class ContentItem
    {
        public ContentItem(object item, Func<object, object> getter)
        {
            dataItem = item;
            contentGetter = getter;
        }

        public object Value
        {
            get { return contentGetter(dataItem); }
        }

        private readonly object dataItem;
        private readonly Func<object, object> contentGetter;
    }
}
