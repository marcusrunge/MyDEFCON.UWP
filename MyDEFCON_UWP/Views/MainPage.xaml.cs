
using MyDEFCON_UWP.ViewModels;
using Unity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = (Application.Current as App).Container.Resolve<MainViewModel>();

        public MainPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
