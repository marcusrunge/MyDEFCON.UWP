namespace ToastNotifications
{
    public class ToastNotificationsFactory
    {
        private static IToastNotifications _toastNotifications;
        public static IToastNotifications Create() => _toastNotifications ?? (_toastNotifications = new ToastNotifications());
    }
}
