namespace ToastNotifications
{
    public interface IToastNotificationsFactory
    {
        IToastNotifications Create();
    }

    public class ToastNotificationsFactory : IToastNotificationsFactory
    {
        private static IToastNotifications _toastNotifications;
        public IToastNotifications Create() => _toastNotifications ?? (_toastNotifications = new ToastNotifications());
    }
}
