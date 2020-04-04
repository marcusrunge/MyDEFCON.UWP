
using MyDEFCON_UWP.ViewModels;
using Unity;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class ChecklistPage : Page
    {
        public ChecklistViewModel ViewModel { get; } = (Application.Current as App).Container.Resolve<ChecklistViewModel>();

        public ChecklistPage()
        {
            InitializeComponent();
            DataContext = ViewModel;
        }
    }
}
