using System;

using MyDEFCON_UWP.ViewModels;
using Windows.UI.Xaml.Controls;
using Unity;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class MessagesPage : Page
    {
        public MessagesViewModel ViewModel { get; } = (Application.Current as App).Container.Resolve<MessagesViewModel>();

        public MessagesPage()
        {
            InitializeComponent();
        }
    }
}
