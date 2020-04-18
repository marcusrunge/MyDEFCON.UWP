using Microsoft.Xaml.Interactivity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Behaviors
{
    public class CommandBarBehavior : Behavior<Microsoft.UI.Xaml.Controls.NavigationView>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.SelectionChanged += (s, e) =>
            {
                if (e.SelectedItem != null && (e.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem).Content != null)
                {
                    switch ((e.SelectedItem as Microsoft.UI.Xaml.Controls.NavigationViewItem).Content)
                    {
                        case "Main":
                            VisualStateManager.GoToState((AssociatedObject.Parent as Grid).Parent as Control, "ShareDefconState", true);
                            break;
                        case "Checklist":
                            VisualStateManager.GoToState((AssociatedObject.Parent as Grid).Parent as Control, "AddItemState", true);
                            break;
                        default:
                            VisualStateManager.GoToState((AssociatedObject.Parent as Grid).Parent as Control, "ClearState", true);
                            break;
                    }
                }
            };
        }
    }
}
