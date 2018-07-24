using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace MyDEFCON_UWP.Converters
{
    public class SelectedItemsToListConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var list = new List<CheckListItem>();
            //string s = (((parameter as ListView).SelectedItems)[0] as CheckListItem).Item.ToString;
            foreach (var item in ((parameter as ListView).SelectedItems))
            {
                list.Add(item as CheckListItem);
                //if((item as CheckListItem).Item != null)
                //Debug.WriteLine("Converter: " + (item as CheckListItem).Item.ToString());
            }
            //return (parameter as ListView).SelectedItems;
            return list;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (parameter as ListView).SelectedItems;
        }
    }
}
