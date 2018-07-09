using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MyDEFCON_UWP.Dependencies
{
    public class FlyoutDependencyProperty
    {
        public static readonly DependencyProperty IsVisibleProperty = DependencyProperty.RegisterAttached(
            "IsOpen",
            typeof(bool),
            typeof(FlyoutDependencyProperty),
            new PropertyMetadata(true, IsOpenChangedCallback));

        public static readonly DependencyProperty ParentProperty = DependencyProperty.RegisterAttached(
            "Parent",
            typeof(FrameworkElement),
            typeof(FlyoutDependencyProperty), null);

        public static void SetIsOpen(DependencyObject element, bool value)
        {
            element.SetValue(IsVisibleProperty, value);
        }

        public static bool GetIsOpen(DependencyObject element)
        {
            return (bool)element.GetValue(IsVisibleProperty);
        }

        private static void IsOpenChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fb = d as Windows.UI.Xaml.Controls.Primitives.FlyoutBase;
            if (fb == null)
                return;

            if ((bool)e.NewValue)
            {
                fb.Closed += flyout_Closed;
                fb.ShowAt(GetParent(d));
            }
            else
            {
                fb.Closed -= flyout_Closed;
                fb.Hide();
            }
        }

        private static void flyout_Closed(object sender, object e)
        {
            // When the flyout is closed, sets its IsOpen attached property to false.
            SetIsOpen(sender as DependencyObject, false);
        }

        public static void SetParent(DependencyObject element, FrameworkElement value)
        {
            element.SetValue(ParentProperty, value);
        }

        public static FrameworkElement GetParent(DependencyObject element)
        {
            return (FrameworkElement)element.GetValue(ParentProperty);
        }
    }

    public class OpenFlyoutAction : DependencyObject, IAction
    {
        public object Execute(object sender, object parameter)
        {
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
            return null;
        }
    }
}
