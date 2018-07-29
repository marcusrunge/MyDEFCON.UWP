using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

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
                            datagramSocket.MessageReceived += async (s, e) =>
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
                                    if(parsedDefconStatus == 0)
                                    {
                                        try
                                        {
                                            using (var streamSocket = new StreamSocket())
                                            {
                                                await streamSocket.ConnectAsync(e.RemoteAddress, "4537");
                                                string response = String.Empty;
                                                using (Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                                                {
                                                    using (StreamReader streamReader = new StreamReader(inputStream))
                                                    {
                                                        response = await streamReader.ReadLineAsync();
                                                        var checkListItems = JsonConvert.DeserializeObject<List<CheckListItem>>(response);
                                                        var defcon1CheckListItems = await CheckListService.LoadCheckList(1);
                                                        var defcon2CheckListItems = await CheckListService.LoadCheckList(2);
                                                        var defcon3CheckListItems = await CheckListService.LoadCheckList(3);
                                                        var defcon4CheckListItems = await CheckListService.LoadCheckList(4);
                                                        var defcon5CheckListItems = await CheckListService.LoadCheckList(5);
                                                        foreach (var item in checkListItems)
                                                        {
                                                            bool itemFound = false;
                                                            if (item.DefconStatus == 1)
                                                            {
                                                                for (int i = 0; i < defcon1CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon1CheckListItems[i].UnixTimeStamp == item.UnixTimeStamp)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon1CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon1CheckListItems[i].Deleted = true;
                                                                            defcon1CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon1CheckListItems.Add(item);
                                                            }

                                                            else if (item.DefconStatus == 2)
                                                            {
                                                                for (int i = 0; i < defcon2CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon2CheckListItems[i].UnixTimeStamp == item.UnixTimeStamp)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon2CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon2CheckListItems[i].Deleted = true;
                                                                            defcon2CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon2CheckListItems.Add(item);
                                                            }

                                                            else if (item.DefconStatus == 3)
                                                            {
                                                                for (int i = 0; i < defcon3CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon3CheckListItems[i].UnixTimeStamp == item.UnixTimeStamp)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon3CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon3CheckListItems[i].Deleted = true;
                                                                            defcon3CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon3CheckListItems.Add(item);
                                                            }

                                                            else if (item.DefconStatus == 4)
                                                            {
                                                                for (int i = 0; i < defcon4CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon4CheckListItems[i].UnixTimeStamp == item.UnixTimeStamp)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon4CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon4CheckListItems[i].Deleted = true;
                                                                            defcon4CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon4CheckListItems.Add(item);
                                                            }

                                                            else if (item.DefconStatus == 5)
                                                            {
                                                                for (int i = 0; i < defcon5CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon5CheckListItems[i].UnixTimeStamp == item.UnixTimeStamp)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon5CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon5CheckListItems[i].Deleted = true;
                                                                            defcon5CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon5CheckListItems.Add(item);
                                                            }
                                                        }
                                                        await CheckListService.SaveCheckList(defcon1CheckListItems, 1);
                                                        await CheckListService.SaveCheckList(defcon2CheckListItems, 2);
                                                        await CheckListService.SaveCheckList(defcon3CheckListItems, 3);
                                                        await CheckListService.SaveCheckList(defcon4CheckListItems, 4);
                                                        await CheckListService.SaveCheckList(defcon5CheckListItems, 5);
                                                    }
                                                }
                                                await streamSocket.CancelIOAsync();
                                                streamSocket.Dispose();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
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
