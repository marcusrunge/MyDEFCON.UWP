using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface ISubscribe
    {
        event EventHandler AppBarButtonClicked;
        event EventHandler ChecklistChanged;
        event EventHandler PaneDisplayModeChangeChanged;
    }
}
