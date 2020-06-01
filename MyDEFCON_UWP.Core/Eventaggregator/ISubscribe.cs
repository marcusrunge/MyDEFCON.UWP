using System;
using System.Collections.Generic;
using System.Text;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface ISubscribe
    {
        event EventHandler AppBarButtonClicked;
        event EventHandler ChecklistChanged;
        event EventHandler PaneDisplayModeChangeChanged;
    }
}
