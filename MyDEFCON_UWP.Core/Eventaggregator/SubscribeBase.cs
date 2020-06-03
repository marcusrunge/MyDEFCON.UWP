using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal abstract class SubscribeBase : ISubscribe
    {
        private static SubscribeBase _instance;

        internal protected delegate void OnAppBarButtonClickedDelegate(object s, IAppBarButtonClickedEventArgs e);
        internal protected OnAppBarButtonClickedDelegate _onAppBarButtonClickedDelegate =(s, e) => _instance.AppBarButtonClicked?.Invoke(s, e);

        internal protected delegate void OnChecklistChangedDelegate(object s, EventArgs e);
        internal protected OnChecklistChangedDelegate _onChecklistChangedDelegate = (s, e) => _instance.ChecklistChanged?.Invoke(s, e);

        internal protected delegate void OnPaneDisplayModeChangeChangedDelegate(object s, IPaneDisplayModeChangedEventArgs e);
        internal protected OnPaneDisplayModeChangeChangedDelegate _onPaneDisplayModeChangeChangedDelegate = (s, e) => _instance.PaneDisplayModeChangeChanged?.Invoke(s, e);

        public event EventHandler<IAppBarButtonClickedEventArgs> AppBarButtonClicked;
        public event EventHandler ChecklistChanged;
        public event EventHandler<IPaneDisplayModeChangedEventArgs> PaneDisplayModeChangeChanged;

        public SubscribeBase()
        {
            _instance = this;
        }
    }
}
