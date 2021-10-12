using MyDEFCON_UWP.ViewModels;
using Unity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Views
{
    // TODO WTS: Change the icons and titles for all NavigationViewItems in ShellPage.xaml.
    public sealed partial class ShellPage : Page
    {
        public ShellViewModel ViewModel { get; } = (Application.Current as App).Container.Resolve<ShellViewModel>();

        public ShellPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
            ViewModel.Initialize(shellFrame, navigationView, KeyboardAccelerators);
        }
    }
}