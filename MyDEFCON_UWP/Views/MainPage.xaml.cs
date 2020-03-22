using System;

using MyDEFCON_UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
