//using System.Diagnostics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace MyDEFCON_UWP.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class CheckListPage : Page
    {
        public CheckListPage()
        {
            this.InitializeComponent();
            //if (IsMobile) flyoutGrid.Margin = new Thickness(0, 0, 0, 3840);
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            ((sender as TextBox).GetBindingExpression(TextBox.TextProperty) as BindingExpression).UpdateSource();
        }

        public bool IsMobile
        {
            get
            {
                var qualifiers = Windows.ApplicationModel.Resources.Core.ResourceContext.GetForCurrentView().QualifierValues;
                return (qualifiers.ContainsKey("DeviceFamily") && qualifiers["DeviceFamily"] == "Mobile");
            }
        }

        //private void defcon1ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    Debug.WriteLine("List: " + e.NewSize.Width.ToString());
        //}
    }
}