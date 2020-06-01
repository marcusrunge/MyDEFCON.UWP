using System;
using System.Collections.Generic;
using System.Text;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class Publish : SubscribeBase, IPublish
    {
        private SubscribeBase _subscribe;
        private static IPublish _publish;
        internal static IPublish Create(SubscribeBase subscribe) => _publish ?? (_publish = new Publish(subscribe));

        public Publish(SubscribeBase subscribe)
        {
            _subscribe = subscribe;
        }

        public void OnAppBarButtonClicked(IAppBarButtonClickedEventArgs eventArgs) => AppBarButtonClicked?.Invoke(this, eventArgs);

        public void OnChecklistChanged(EventArgs eventArgs) => ChecklistChanged?.Invoke(this, eventArgs);

        public void OnPaneDisplayModeChangeChanged(IPaneDisplayModeChangedEventArgs paneDisplayModeChangedEventArgs) => PaneDisplayModeChangeChanged?.Invoke(this, paneDisplayModeChangedEventArgs);
    }
}
