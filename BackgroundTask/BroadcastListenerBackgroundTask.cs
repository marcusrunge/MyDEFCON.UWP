using Services;
using System;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.UI.Notifications;

namespace BackgroundTask
{
    public sealed class BroadcastListenerBackgroundTask : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            var backgroundWorkCost = BackgroundWorkCost.CurrentBackgroundWorkCost;
            if (backgroundWorkCost == BackgroundWorkCostValue.High) return;
            else
            {
                var deferral = taskInstance.GetDeferral();
                try
                {
                    var details = taskInstance.TriggerDetails as SocketActivityTriggerDetails;
                    var socketInformation = details.SocketInformation;
                    switch (details.Reason)
                    {
                        case SocketActivityTriggerReason.SocketActivity:
                            var datagramSocket = socketInformation.DatagramSocket;
                            datagramSocket.MessageReceived += (s, e) =>
                                {
                                    var dataReader = e.GetDataReader();
                                    var defconStatus = dataReader.ReadString(dataReader.UnconsumedBufferLength);
                                    int.TryParse(defconStatus, out int parsedDefconStatus);
                                    if (parsedDefconStatus > 0 && parsedDefconStatus < 6)
                                    {
                                        ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                                        int savedDefconStatus = 0;
                                        if (roamingSettings.Values.ContainsKey("defconStatus")) savedDefconStatus = Convert.ToInt16(roamingSettings.Values["defconStatus"].ToString());                                        
                                        if (parsedDefconStatus != savedDefconStatus)
                                        {
                                            roamingSettings.Values["defconStatus"] = defconStatus;
                                            ShowToast("DEFCON " + defconStatus);
                                            LiveTileService.SetLiveTile(int.Parse(defconStatus), LoadUseTransparentTileSetting());
                                        }
                                    }
                                };
                            await datagramSocket.CancelIOAsync();
                            datagramSocket.TransferOwnership("myDefconSocket");
                            break;
                        case SocketActivityTriggerReason.SocketClosed:
                            DatagramSocket newDatagramSocket = new DatagramSocket();
                            newDatagramSocket.EnableTransferOwnership(taskInstance.Task.TaskId, SocketActivityConnectedStandbyAction.DoNotWake);
                            await newDatagramSocket.BindServiceNameAsync("4536");
                            await newDatagramSocket.CancelIOAsync();
                            newDatagramSocket.TransferOwnership("myDefconSocket");
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception)
                {
                    try
                    {
                        DatagramSocket newDatagramSocket = new DatagramSocket();
                        newDatagramSocket.EnableTransferOwnership(taskInstance.Task.TaskId, SocketActivityConnectedStandbyAction.DoNotWake);
                        await newDatagramSocket.BindServiceNameAsync("4536");
                        await newDatagramSocket.CancelIOAsync();
                        newDatagramSocket.TransferOwnership("myDefconSocket");
                    }
                    catch (Exception) { }
                }
                deferral.Complete();
            }
        }

        private void ShowToast(string text)
        {
            var toastNotifier = ToastNotificationManager.CreateToastNotifier();
            var toastXml = ToastNotificationManager.GetTemplateContent(ToastTemplateType.ToastText02);
            var textNodes = toastXml.GetElementsByTagName("text");
            textNodes.First().AppendChild(toastXml.CreateTextNode(text));
            var toastNotification = new ToastNotification(toastXml);
            toastNotifier.Show(new ToastNotification(toastXml));
        }

        private bool LoadUseTransparentTileSetting()
        {
            ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("useTransparentTile")) return (bool)localSettings.Values["useTransparentTile"];
            else return false;
        }
    }
}
