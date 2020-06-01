using System;
using System.Collections.Generic;
using System.Text;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class PaneDisplayModeChangedEventArgs : IPaneDisplayModeChangedEventArgs
    {
        public int Mode { get; }
        public PaneDisplayModeChangedEventArgs(int mode) => Mode = mode;
    }
}
