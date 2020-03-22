using System;

using MyDEFCON_UWP.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Unity;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class ChecklistPage : Page
    {
        public ChecklistViewModel ViewModel { get; } = (Application.Current as App).Container.Resolve < ChecklistViewModel>();

        public ChecklistPage()
        {
            InitializeComponent();
        }
    }
}
