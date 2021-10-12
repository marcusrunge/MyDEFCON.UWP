namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class EventAggregator : IEventAggregator
    {
        public ISubscribe Subscribe => Eventaggregator.Subscribe.Create();

        public IPublish Publish => Eventaggregator.Publish.Create(Subscribe as SubscribeBase);
    }
}