using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToastNotifications
{
    public class ToastNotificationsFactory
    {
        private static IToastNotifications _toastNotifications;
        public static IToastNotifications Create() => _toastNotifications ?? (_toastNotifications = new ToastNotifications());
    }
}
