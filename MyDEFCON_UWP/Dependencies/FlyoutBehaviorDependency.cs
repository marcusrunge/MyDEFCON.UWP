using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MyDEFCON_UWP.Dependencies
{
    public class FlyoutBehavior : DependencyObject, IBehavior
    {
        public FlyoutBehavior()
        {
            OpenActions = new ActionCollection();
            CloseActions = new ActionCollection();
        }
        public DependencyObject AssociatedObject { get; private set; }

        public void Attach(DependencyObject associatedObject)
        {
            var flyout = associatedObject as FlyoutBase;
            if (flyout == null) throw new ArgumentException("FlyoutBehavior can only be attached to FlyoutBase");
            AssociatedObject = associatedObject;
            flyout.Opened += (o, e) =>
            {
                foreach (IAction action in OpenActions)
                {
                    action.Execute(AssociatedObject, null);
                }
            };
            flyout.Closed += (o, e) =>
            {
                foreach (IAction action in CloseActions)
                {
                    action.Execute(AssociatedObject, null);
                }
            };
        }

        public void Detach()
        {
            AssociatedObject = null;
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OpenActionsProperty =
            DependencyProperty.Register("OpenActions", typeof(ActionCollection), typeof(FlyoutBehavior), new PropertyMetadata(null));

        public static readonly DependencyProperty CloseActionsProperty =
            DependencyProperty.Register("CloseActions", typeof(ActionCollection), typeof(FlyoutBehavior), new PropertyMetadata(null));

        public ActionCollection OpenActions
        {
            get { return GetValue(OpenActionsProperty) as ActionCollection; }
            set { SetValue(OpenActionsProperty, value); }
        }
        public ActionCollection CloseActions
        {
            get { return GetValue(CloseActionsProperty) as ActionCollection; }
            set { SetValue(CloseActionsProperty, value); }
        }

    }
}
