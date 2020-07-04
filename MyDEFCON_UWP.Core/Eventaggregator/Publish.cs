using System;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class Publish : IPublish
    {
        private static IPublish _publish;
        private readonly SubscribeBase _subscribeBase;

        public Publish(SubscribeBase subscribeBase)
        {
            _subscribeBase = subscribeBase;
        }

        internal static IPublish Create(SubscribeBase subscribeBase) => _publish ?? (_publish = new Publish(subscribeBase));

        public void OnAppBarButtonClicked(IAppBarButtonClickedEventArgs appBarButtonClickedEventArgs) => _subscribeBase._onAppBarButtonClickedDelegate(this, appBarButtonClickedEventArgs);

        public void OnChecklistChanged(EventArgs eventArgs) => _subscribeBase._onChecklistChangedDelegate(this, eventArgs);

        public void OnPaneDisplayModeChangeChanged(IPaneDisplayModeChangedEventArgs paneDisplayModeChangedEventArgs) => _subscribeBase._onPaneDisplayModeChangeChangedDelegate(this, paneDisplayModeChangedEventArgs);
    }
}
