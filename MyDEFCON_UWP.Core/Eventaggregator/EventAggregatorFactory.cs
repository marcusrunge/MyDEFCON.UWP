namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface IEventAggregatorFactory
    {
        IEventAggregator Create();
    }

    public class EventAggregatorFactory : IEventAggregatorFactory
    {
        private static IEventAggregator _eventAggregator;
        public IEventAggregator Create() => _eventAggregator ?? (_eventAggregator = new EventAggregator());
    }
}
