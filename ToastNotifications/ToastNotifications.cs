namespace ToastNotifications
{

    public interface IToastNotifications
    {
        IInfo Info { get; }
    }

    internal class ToastNotifications : ToastNotificationsBase
    {
        internal ToastNotifications() : base()
        {
            _info = global::ToastNotifications.Info.Create(this);
        }
    }
}
