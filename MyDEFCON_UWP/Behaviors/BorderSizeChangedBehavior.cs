using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Behaviors
{
    public class BorderSizeChangedBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject is Border border) border.SizeChanged += AssociatedObject_SizeChanged;
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            TextBlock textBlock = ((Border)sender).Child as TextBlock;

            int length = 1;
            var fontSize = Math.Floor(e.NewSize.Height / 1.5);

            length = textBlock.Text.Length;

            double requiredTextBlockWidth = fontSize * length;

            if (e.NewSize.Width > requiredTextBlockWidth) fontSize = Math.Floor(e.NewSize.Height / 1.5);
            else fontSize = Math.Floor(e.NewSize.Width / length);

            textBlock.FontSize = fontSize > 0 ? fontSize : 1;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is Border border) border.SizeChanged -= AssociatedObject_SizeChanged;
            base.OnDetaching();
        }
    }
}