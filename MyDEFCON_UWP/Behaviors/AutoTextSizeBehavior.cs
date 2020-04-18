using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Behaviors
{
    public class AutoTextSizeBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is RadioButton) ((RadioButton)AssociatedObject).SizeChanged += AssociatedObject_SizeChanged;

        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            int length = 1;
            var fontSize = Math.Floor(e.NewSize.Height / 1.5);

            if (AssociatedObject is RadioButton) length = ((RadioButton)AssociatedObject).Content.ToString().Length;

            double requiredTextBlockWidth = fontSize * length;

            if (e.NewSize.Width > requiredTextBlockWidth) fontSize = Math.Floor(e.NewSize.Height / 1.5);
            else fontSize = Math.Floor(e.NewSize.Width / length);

            if (AssociatedObject is RadioButton) ((RadioButton)AssociatedObject).FontSize = fontSize > 0 ? fontSize : 1;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is RadioButton) ((RadioButton)AssociatedObject).SizeChanged -= AssociatedObject_SizeChanged;
            base.OnDetaching();
        }
    }
}
