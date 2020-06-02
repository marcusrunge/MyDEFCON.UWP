namespace MyDEFCON_UWP.Core.Eventaggregator
{
    internal class Subscribe:SubscribeBase
    {
        private static ISubscribe _subscribe;
        internal static ISubscribe Create() => _subscribe ?? (_subscribe = new Subscribe());
    }
}
