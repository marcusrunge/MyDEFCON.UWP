using Windows.UI.Notifications;

namespace ToastNotifications
{
    internal abstract class ToastNotificationsBase : IToastNotifications
    {
        protected IInfo _info;
        public IInfo Info => _info;

        protected internal void ShowToastNotification(ToastNotification toastNotification)
        {
            try
            {
                ToastNotificationManager.CreateToastNotifier().Show(toastNotification);
            }
            catch { }
        }
    }
}