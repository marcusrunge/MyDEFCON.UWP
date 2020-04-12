using Microsoft.Xaml.Interactivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Behaviors
{
    public class AutoTextSizeBehavior: Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            if(AssociatedObject is RadioButton) ((RadioButton)AssociatedObject).SizeChanged += AssociatedObject_SizeChanged;
            
        }

        private void AssociatedObject_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var fontSize = Math.Floor(e.NewSize.Height / 1.5);
            if (AssociatedObject is RadioButton) ((RadioButton)AssociatedObject).FontSize= fontSize > 0 ? fontSize : 1;
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject is RadioButton) ((RadioButton)AssociatedObject).SizeChanged -= AssociatedObject_SizeChanged;
            base.OnDetaching();
        }
    }
}
