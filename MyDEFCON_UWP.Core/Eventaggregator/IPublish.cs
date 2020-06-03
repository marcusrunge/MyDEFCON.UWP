using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface IPublish
    {
        void OnChecklistChanged(EventArgs eventArgs);
        void OnAppBarButtonClicked(IAppBarButtonClickedEventArgs eventArgs);
        void OnPaneDisplayModeChangeChanged(IPaneDisplayModeChangedEventArgs paneDisplayModeChangedEventArgs);
    }
}
