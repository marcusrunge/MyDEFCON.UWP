namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public static class EventAggregatorFactory
    {
        private static IEventAggregator _eventAggregator;
        public static IEventAggregator Create() => _eventAggregator ?? (_eventAggregator = new EventAggregator());
    }
}
