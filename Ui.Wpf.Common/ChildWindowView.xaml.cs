using MahApps.Metro.SimpleChildWindow;
using System;
using System.Windows.Data;
using Ui.Wpf.Common.ShowOptions;

namespace Ui.Wpf.Common
{
    /// <summary>
    /// Interaction logic for ChildWindowView.xaml
    /// </summary>
    public partial class ChildWindowView : ChildWindow
    {
        public ChildWindowView(UiShowChildWindowOptions options)
        {
            DataContext = options;
            InitializeComponent();
        }
    }

    internal class NullableIntToDoubleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value == null
                ? double.NaN
                : System.Convert.ToDouble(value);
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            System.Globalization.CultureInfo culture)
        {
            return double.IsNaN(System.Convert.ToDouble(value))
                ? (int?) null
                : System.Convert.ToInt32(value);
        }
    }
}