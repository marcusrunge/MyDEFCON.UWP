using MyDEFCON_UWP.ViewModels;
using Unity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// Die Elementvorlage "Leere Seite" wird unter https://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace MyDEFCON_UWP.Views
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet oder zu der innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class FullScreenPage : Page
    {
        public FullScreenViewModel ViewModel { get; } = (Application.Current as App).Container.Resolve<FullScreenViewModel>();

        public FullScreenPage()
        {
            this.InitializeComponent();
        }
    }
}