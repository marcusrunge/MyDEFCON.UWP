using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface ISubscribe
    {
        event EventHandler<IAppBarButtonClickedEventArgs> AppBarButtonClicked;
        event EventHandler ChecklistChanged;
        event EventHandler<IPaneDisplayModeChangedEventArgs> PaneDisplayModeChangeChanged;
    }
}
