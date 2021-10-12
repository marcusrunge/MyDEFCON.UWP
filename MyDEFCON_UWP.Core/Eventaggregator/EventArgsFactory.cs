namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public class EventArgsFactory
    {
        public static T CreateEventArgs<T>(object args) where T : class
        {
            if (typeof(T) == typeof(IAppBarButtonClickedEventArgs)) return new AppBarButtonClickedEventArgs((string)args) as T;
            else if (typeof(T) == typeof(IPaneDisplayModeChangedEventArgs)) return new PaneDisplayModeChangedEventArgs((int)args) as T;
            else return null;
        }
    }
}