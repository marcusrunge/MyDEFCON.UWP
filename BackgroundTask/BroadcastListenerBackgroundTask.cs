﻿using Checklists;
using LiveTile;
using Models;
using Newtonsoft.Json;
using Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity;
using Windows.ApplicationModel.Background;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.UI.Xaml;

namespace BackgroundTask
{
    public sealed class BroadcastListenerBackgroundTask : IBackgroundTask
    {
        private IUnityContainer _container;

        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                _container.RegisterType<IStorageFactory, StorageFactory>();
                _container.RegisterFactory<IStorage>((c) => c.Resolve<IStorageFactory>().Create(), FactoryLifetime.Singleton);
                _container.RegisterType<IChecklistsFactory, ChecklistsFactory>();
                _container.RegisterFactory<IChecklists>((c) => c.Resolve<IChecklistsFactory>().Create(), FactoryLifetime.Singleton);
                _container.RegisterType<ILiveTileFactory, LiveTileFactory>();
                _container.RegisterFactory<ILiveTile>((c) => c.Resolve<ILiveTileFactory>().Create(), FactoryLifetime.Singleton);
            }
            var checklists = _container.Resolve<IChecklists>();
            var liveTile = _container.Resolve<ILiveTile>();
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
                                            liveTile.DefconTile.SetTile(int.Parse(defconStatus));
                                        }
                                    }
                                    if (parsedDefconStatus == 0)
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
                                                        var defcon1CheckListItems = checklists.Collection.Defcon1Checklist;
                                                        var defcon2CheckListItems = checklists.Collection.Defcon2Checklist;
                                                        var defcon3CheckListItems = checklists.Collection.Defcon3Checklist;
                                                        var defcon4CheckListItems = checklists.Collection.Defcon4Checklist;
                                                        var defcon5CheckListItems = checklists.Collection.Defcon5Checklist;
                                                        foreach (var item in checkListItems)
                                                        {
                                                            bool itemFound = false;
                                                            if (item.DefconStatus == 1)
                                                            {
                                                                for (int i = 0; i < defcon1CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon1CheckListItems[i].UnixTimeStampCreated == item.UnixTimeStampCreated)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon1CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon1CheckListItems[i].Deleted = true;
                                                                            defcon1CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                        else if (defcon1CheckListItems[i].UnixTimeStampUpdated < item.UnixTimeStampUpdated)
                                                                        {
                                                                            defcon1CheckListItems[i].Item = item.Item;
                                                                            defcon1CheckListItems[i].Checked = item.Checked;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon1CheckListItems.Add(item);
                                                            }
                                                            else if (item.DefconStatus == 2)
                                                            {
                                                                for (int i = 0; i < defcon2CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon2CheckListItems[i].UnixTimeStampCreated == item.UnixTimeStampCreated)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon2CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon2CheckListItems[i].Deleted = true;
                                                                            defcon2CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                        else if (defcon2CheckListItems[i].UnixTimeStampUpdated < item.UnixTimeStampUpdated)
                                                                        {
                                                                            defcon2CheckListItems[i].Item = item.Item;
                                                                            defcon2CheckListItems[i].Checked = item.Checked;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon2CheckListItems.Add(item);
                                                            }
                                                            else if (item.DefconStatus == 3)
                                                            {
                                                                for (int i = 0; i < defcon3CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon3CheckListItems[i].UnixTimeStampCreated == item.UnixTimeStampCreated)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon3CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon3CheckListItems[i].Deleted = true;
                                                                            defcon3CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                        else if (defcon3CheckListItems[i].UnixTimeStampUpdated < item.UnixTimeStampUpdated)
                                                                        {
                                                                            defcon3CheckListItems[i].Item = item.Item;
                                                                            defcon3CheckListItems[i].Checked = item.Checked;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon3CheckListItems.Add(item);
                                                            }
                                                            else if (item.DefconStatus == 4)
                                                            {
                                                                for (int i = 0; i < defcon4CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon4CheckListItems[i].UnixTimeStampCreated == item.UnixTimeStampCreated)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon4CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon4CheckListItems[i].Deleted = true;
                                                                            defcon4CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                        else if (defcon4CheckListItems[i].UnixTimeStampUpdated < item.UnixTimeStampUpdated)
                                                                        {
                                                                            defcon4CheckListItems[i].Item = item.Item;
                                                                            defcon4CheckListItems[i].Checked = item.Checked;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon4CheckListItems.Add(item);
                                                            }
                                                            else if (item.DefconStatus == 5)
                                                            {
                                                                for (int i = 0; i < defcon5CheckListItems.Count; i++)
                                                                {
                                                                    if (defcon5CheckListItems[i].UnixTimeStampCreated == item.UnixTimeStampCreated)
                                                                    {
                                                                        itemFound = true;
                                                                        if (defcon5CheckListItems[i].Deleted != item.Deleted)
                                                                        {
                                                                            defcon5CheckListItems[i].Deleted = true;
                                                                            defcon5CheckListItems[i].Visibility = Visibility.Collapsed;
                                                                        }
                                                                        else if (defcon5CheckListItems[i].UnixTimeStampUpdated < item.UnixTimeStampUpdated)
                                                                        {
                                                                            defcon5CheckListItems[i].Item = item.Item;
                                                                            defcon5CheckListItems[i].Checked = item.Checked;
                                                                        }
                                                                    }
                                                                }
                                                                if (!itemFound) defcon5CheckListItems.Add(item);
                                                            }
                                                        }
                                                        await checklists.Operations.SaveCheckList(defcon1CheckListItems, 1);
                                                        await checklists.Operations.SaveCheckList(defcon2CheckListItems, 2);
                                                        await checklists.Operations.SaveCheckList(defcon3CheckListItems, 3);
                                                        await checklists.Operations.SaveCheckList(defcon4CheckListItems, 4);
                                                        await checklists.Operations.SaveCheckList(defcon5CheckListItems, 5);
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
                            datagramSocket.TransferOwnership("myDefconDatagramSocket");
                            break;

                        case SocketActivityTriggerReason.SocketClosed:
                            DatagramSocket newDatagramSocket = new DatagramSocket();
                            newDatagramSocket.EnableTransferOwnership(taskInstance.Task.TaskId, SocketActivityConnectedStandbyAction.DoNotWake);
                            await newDatagramSocket.BindServiceNameAsync("4536");
                            await newDatagramSocket.CancelIOAsync();
                            newDatagramSocket.TransferOwnership("myDefconDatagramSocket");
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
                        newDatagramSocket.TransferOwnership("myDefconDatagramSocket");
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
            toastNotifier.Show(new ToastNotification(toastXml));
        }

        private bool LoadUseTransparentTileSetting()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("useTransparentTile")) return (bool)localSettings.Values["useTransparentTile"];
            else return false;
        }
    }
}