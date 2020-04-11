using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Behaviors
{
    public class VisualStateBehavior : Behavior<Page>
    {
        public string VisualState
        {
            get { return (string)GetValue(VisualStateProperty); }
            set { SetValue(VisualStateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty VisualStateProperty =
            DependencyProperty.Register("VisualState", typeof(int), typeof(VisualStateBehavior), new PropertyMetadata(null, (d,e)=>
            {
                VisualStateManager.GoToState((d as VisualStateBehavior).AssociatedObject, e.NewValue.ToString(), true);
            }));
    }
}
