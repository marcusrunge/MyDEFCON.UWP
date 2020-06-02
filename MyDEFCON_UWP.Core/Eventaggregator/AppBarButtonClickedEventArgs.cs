namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class AppBarButtonClickedEventArgs : IAppBarButtonClickedEventArgs
    {
        public string Button { get; }
        public AppBarButtonClickedEventArgs(string button) => Button = button;
    }
}
