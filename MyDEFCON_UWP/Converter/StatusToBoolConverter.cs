using System;
using Windows.UI.Xaml.Data;

namespace MyDEFCON_UWP.Converter
{
    public class StatusToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language) => (int)value == int.Parse(parameter as string);

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
