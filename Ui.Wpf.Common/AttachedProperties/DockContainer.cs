using System.Windows;

namespace Ui.Wpf.Common.AttachedProperties
{
    public static class DockContainer
    {
        public static readonly DependencyProperty NameProperty = DependencyProperty.RegisterAttached(
            "Name", typeof(string), typeof(DockContainer), new PropertyMetadata(default(string)));

        public static void SetName(DependencyObject element, string value)
        {
            element.SetValue(NameProperty, value);
        }

        public static string GetName(DependencyObject element)
        {
            return (string) element.GetValue(NameProperty);
        }
    }
}