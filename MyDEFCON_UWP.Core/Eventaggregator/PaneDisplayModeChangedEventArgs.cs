namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class PaneDisplayModeChangedEventArgs : IPaneDisplayModeChangedEventArgs
    {
        public int Mode { get; }
        public PaneDisplayModeChangedEventArgs(int mode) => Mode = mode;
    }
}
