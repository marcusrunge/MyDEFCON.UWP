using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace MyDEFCON_UWP.Converters
{
    public class IsCheckedToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return (bool?)value;
            }
            return DependencyProperty.UnsetValue;

        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool)
            {
                return (bool?)value;
            }
            return DependencyProperty.UnsetValue;

        }
    }
}
