using System;

using MyDEFCON_UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class ChecklistPage : Page
    {
        public ChecklistViewModel ViewModel { get; } = new ChecklistViewModel();

        public ChecklistPage()
        {
            InitializeComponent();
        }
    }
}
