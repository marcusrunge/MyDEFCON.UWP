using System;
using System.Collections.Generic;
using System.Text;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal abstract class SubscribeBase : ISubscribe
    {
        public event EventHandler AppBarButtonClicked;
        public event EventHandler ChecklistChanged;
        public event EventHandler PaneDisplayModeChangeChanged;
    }
}
