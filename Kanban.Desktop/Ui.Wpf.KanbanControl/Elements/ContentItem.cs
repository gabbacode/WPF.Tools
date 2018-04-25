using System;
using System.ComponentModel;
using System.Windows;

namespace Ui.Wpf.KanbanControl.Elements
{
    public class ContentItem : INotifyPropertyChanged
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

        private Visibility visibility = Visibility.Collapsed;
        public Visibility Visibility
        {
            get
            {
                return visibility;
            }
            set
            {
                visibility = value;
                PropertyChanged(this, new PropertyChangedEventArgs("Visibility"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private readonly object dataItem;
        private readonly Func<object, object> contentGetter;

    }
}
