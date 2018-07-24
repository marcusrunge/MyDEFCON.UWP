using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace MyDEFCON_UWP.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FullScreenPage : Page
    {
        public FullScreenPage()
        {
            this.InitializeComponent();
        }

        private void border_LayoutUpdated(object sender, object e)
        {

        }
    }
}
