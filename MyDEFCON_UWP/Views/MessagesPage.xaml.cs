using System;

using MyDEFCON_UWP.ViewModels;

using Windows.UI.Xaml.Controls;

namespace MyDEFCON_UWP.Views
{
    public sealed partial class MessagesPage : Page
    {
        public MessagesViewModel ViewModel { get; } = new MessagesViewModel();

        public MessagesPage()
        {
            InitializeComponent();
        }
    }
}
