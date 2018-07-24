using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MyDEFCON_UWP.Converters
{
    public class ListViewWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (parameter as ListView);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
