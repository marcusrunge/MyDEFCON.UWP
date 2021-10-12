using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal abstract class SubscribeBase : ISubscribe
    {
        private static SubscribeBase _instance;

        protected internal delegate void OnAppBarButtonClickedDelegate(object s, IAppBarButtonClickedEventArgs e);

        protected internal OnAppBarButtonClickedDelegate _onAppBarButtonClickedDelegate = (s, e) => _instance.AppBarButtonClicked?.Invoke(s, e);

        protected internal delegate void OnChecklistChangedDelegate(object s, EventArgs e);

        protected internal OnChecklistChangedDelegate _onChecklistChangedDelegate = (s, e) => _instance.ChecklistChanged?.Invoke(s, e);

        protected internal delegate void OnPaneDisplayModeChangeChangedDelegate(object s, IPaneDisplayModeChangedEventArgs e);

        protected internal OnPaneDisplayModeChangeChangedDelegate _onPaneDisplayModeChangeChangedDelegate = (s, e) => _instance.PaneDisplayModeChangeChanged?.Invoke(s, e);

        public event EventHandler<IAppBarButtonClickedEventArgs> AppBarButtonClicked;

        public event EventHandler ChecklistChanged;

        public event EventHandler<IPaneDisplayModeChangedEventArgs> PaneDisplayModeChangeChanged;

        public SubscribeBase()
        {
            _instance = this;
        }
    }
}