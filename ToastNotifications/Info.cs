using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace ToastNotifications
{
    public interface IInfo
    {
        void Show(string title, string content = null);
    }

    internal class Info : IInfo
    {
        private static IInfo _info;

        internal static IInfo Create(ToastNotificationsBase toastNotificationsBase) => _info ?? (_info = new Info(toastNotificationsBase));

        private readonly ToastNotificationsBase _toastNotificationsBase;

        public Info(ToastNotificationsBase toastNotificationsBase)
        {
            _toastNotificationsBase = toastNotificationsBase;
        }

        public void Show(string title, string content = null)
        {
            var toastContent = new ToastContent()
            {
                Launch = "ToastContentActivationParams",

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric()
                    {
                        Children =
                        {
                            new AdaptiveText()
                            {
                                Text = title
                            },

                            new AdaptiveText()
                            {
                                 Text = content
                            }
                        }
                    }
                },
                Duration = ToastDuration.Short
            };

            var toast = new ToastNotification(toastContent.GetXml())
            {
                Tag = "ToastTag"
            };

            _toastNotificationsBase.ShowToastNotification(toast);
        }
    }
}