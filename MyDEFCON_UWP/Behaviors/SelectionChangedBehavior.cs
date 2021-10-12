using Microsoft.Xaml.Interactivity;
using Models;
using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Behaviors
{
    public class SelectionChangedBehavior : Behavior<ListView>
    {
        public List<long> SelectedItems
        {
            get { return (List<long>)GetValue(SelectedItemsProperty); }
            set { SetValue(SelectedItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for SelectedItems.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register("SelectedItems", typeof(List<long>), typeof(SelectionChangedBehavior), new PropertyMetadata(null, (d, e) =>
             {
                 (d as SelectionChangedBehavior).SelectedItems = e.NewValue as List<long>;
             }));

        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        protected override void OnDetaching()
        {
            AssociatedObject.SelectionChanged -= AssociatedObject_SelectionChanged;
            base.OnDetaching();
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            for (int i = 0; i < e.AddedItems.Count; i++) SelectedItems.Add((e.AddedItems[i] as CheckListItem).UnixTimeStampCreated);
            for (int i = 0; i < SelectedItems.Count; i++) for (int j = 0; j < e.RemovedItems.Count; j++) if (SelectedItems[i] == (e.RemovedItems[j] as CheckListItem).UnixTimeStampCreated) SelectedItems.RemoveAt(i);
        }
    }
}