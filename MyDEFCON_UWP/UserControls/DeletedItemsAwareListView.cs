using Models;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.UserControls
{
    public class DeletedItemsAwareListView : ListView
    {
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            FrameworkElement source = element as FrameworkElement;
            source.Visibility = (item as CheckListItem).Visibility;
        }
    }
}