using System;
using System.ComponentModel;
using System.Windows;

namespace Ui.Wpf.KanbanControl.Elements
{
    public class ContentItem : INotifyPropertyChanged
    {
        public ContentItem(object item, Func<object, object> getter)
        {
            DataItem = item;
            contentGetter = getter;
        }

        public object Value
        {
            get { return contentGetter(DataItem); }
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

        internal object DataItem { get; private set; }

        private readonly Func<object, object> contentGetter;

    }
}
