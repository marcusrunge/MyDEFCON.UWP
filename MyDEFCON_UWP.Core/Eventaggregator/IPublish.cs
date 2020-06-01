using System;
using System.Collections.Generic;
using System.Text;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface IPublish
    {
        void OnChecklistChanged(EventArgs eventArgs);
        void OnAppBarButtonClicked(IAppBarButtonClickedEventArgs eventArgs);
        void OnPaneDisplayModeChangeChanged(IPaneDisplayModeChangedEventArgs paneDisplayModeChangedEventArgs)
    }
}
