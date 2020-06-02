namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface IEventAggregator
    {
        ISubscribe Subscribe { get; }
        IPublish Publish { get; }
    }
}
