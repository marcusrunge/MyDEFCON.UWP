using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal abstract class SubscribeBase : ISubscribe
    {
        private static SubscribeBase _instance;

        internal delegate void OnAppBarButtonClickedDelegate(object s, EventArgs e);
        internal OnAppBarButtonClickedDelegate _onAppBarButtonClickedDelegate =(s, e) => _instance.AppBarButtonClicked?.Invoke(s, e);

        internal delegate void OnChecklistChangedDelegate(object s, EventArgs e);
        internal OnChecklistChangedDelegate _onChecklistChangedDelegate = (s, e) => _instance.ChecklistChanged?.Invoke(s, e);

        internal delegate void OnPaneDisplayModeChangeChangedDelegate(object s, EventArgs e);
        internal OnPaneDisplayModeChangeChangedDelegate _onPaneDisplayModeChangeChangedDelegate = (s, e) => _instance.PaneDisplayModeChangeChanged?.Invoke(s, e);

        public event EventHandler AppBarButtonClicked;
        public event EventHandler ChecklistChanged;
        public event EventHandler PaneDisplayModeChangeChanged;

        public SubscribeBase()
        {
            _instance = this;
        }
    }
}
