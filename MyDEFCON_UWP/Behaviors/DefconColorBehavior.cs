using Microsoft.Xaml.Interactivity;
using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyDEFCON_UWP.Behaviors
{
    public class DefconColorBehavior : Behavior<RadioButton>
    {
        public string ActiveForeground
        {
            get => (string)GetValue(ActiveForegroundProperty);
            set => SetValue(ActiveForegroundProperty, value);
        }

        public static readonly DependencyProperty ActiveForegroundProperty = DependencyProperty.Register("ActiveForeground", typeof(string), typeof(DefconColorBehavior), new PropertyMetadata(0));

        public string ActiveBackground
        {
            get => (string)GetValue(ActiveBackgroundProperty);
            set => SetValue(ActiveBackgroundProperty, value);
        }

        public static readonly DependencyProperty ActiveBackgroundProperty = DependencyProperty.Register("ActiveBackground", typeof(string), typeof(DefconColorBehavior), new PropertyMetadata(0));

        public string PassiveForeground
        {
            get => (string)GetValue(PassiveForegroundProperty);
            set => SetValue(PassiveForegroundProperty, value);
        }

        public static readonly DependencyProperty PassiveForegroundProperty = DependencyProperty.Register("PassiveForeground", typeof(string), typeof(DefconColorBehavior), new PropertyMetadata(0));

        public string PassiveBackground
        {
            get => (string)GetValue(PassiveBackgroundProperty);
            set => SetValue(PassiveBackgroundProperty, value);
        }

        public static readonly DependencyProperty PassiveBackgroundProperty = DependencyProperty.Register("PassiveBackground", typeof(string), typeof(DefconColorBehavior), new PropertyMetadata(0));

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject.IsChecked.Value)
            {
                AssociatedObject.Background = GetSolidColorBrush(ActiveBackground);
                AssociatedObject.Foreground = GetSolidColorBrush(ActiveForeground);
            }
            else
            {
                AssociatedObject.Background = GetSolidColorBrush(PassiveBackground);
                AssociatedObject.Foreground = GetSolidColorBrush(PassiveForeground);
            }
            AssociatedObject.Checked += (s, e) =>
            {
                (s as RadioButton).Background = GetSolidColorBrush(ActiveBackground);
                (s as RadioButton).Foreground = GetSolidColorBrush(ActiveForeground);
            };
            AssociatedObject.Unchecked += (s, e) =>
            {
                (s as RadioButton).Background = GetSolidColorBrush(PassiveBackground);
                (s as RadioButton).Foreground = GetSolidColorBrush(PassiveForeground);
            };
        }

        private SolidColorBrush GetSolidColorBrush(string hex)
        {
            hex = hex.Replace("#", string.Empty);
            byte a = (byte)(Convert.ToUInt32(hex.Substring(0, 2), 16));
            byte r = (byte)(Convert.ToUInt32(hex.Substring(2, 2), 16));
            byte g = (byte)(Convert.ToUInt32(hex.Substring(4, 2), 16));
            byte b = (byte)(Convert.ToUInt32(hex.Substring(6, 2), 16));
            SolidColorBrush myBrush = new SolidColorBrush(Color.FromArgb(a, r, g, b));
            return myBrush;
        }
    }
}