using Models;
using Newtonsoft.Json;
using Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.UI.Xaml;

namespace SocketLibrary
{
    public class StreamSocketService
    {
        public event EventHandler IncomingChecklistReceived;
        public async Task StartListener()
        {
            try
            {
                var streamSocketListener = new StreamSocketListener();
                streamSocketListener.ConnectionReceived += async (s, e) =>
                                {                                    
                                    using (var streamWriter = new StreamWriter(e.Socket.OutputStream.AsStreamForWrite()))
                                    {
                                        await streamWriter.WriteLineAsync(await GetJsonSerializedChecklistItems());
                                        await streamWriter.FlushAsync();
                                    }
                                };
                await streamSocketListener.BindServiceNameAsync("4537");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        public async Task SendStringData(HostName hostName, string data)
        {
            try
            {
                using (var streamSocket = new StreamSocket())
                {
                    await streamSocket.ConnectAsync(hostName, "4537");
                    using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
                    {
                        using (var streamWriter = new StreamWriter(outputStream))
                        {
                            await streamWriter.WriteLineAsync(data);
                            await streamWriter.FlushAsync();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        public async Task<string> ReceiveStringData(HostName hostName)
        {
            string response = String.Empty;
            try
            {
                using (var streamSocket = new StreamSocket())
                {
                    await streamSocket.ConnectAsync(hostName, "4537");

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
                }
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
            }
            OnIncomingChecklistReceived();
            return response;
        }

        private void OnIncomingChecklistReceived() => IncomingChecklistReceived?.Invoke(this, new EventArgs());

        public async Task<string> GetJsonSerializedChecklistItems()
        {
            var checkListItems = new List<CheckListItem>();
            var defcon1CheckListItems = await CheckListService.LoadCheckList(1);
            var defcon2CheckListItems = await CheckListService.LoadCheckList(2);
            var defcon3CheckListItems = await CheckListService.LoadCheckList(3);
            var defcon4CheckListItems = await CheckListService.LoadCheckList(4);
            var defcon5CheckListItems = await CheckListService.LoadCheckList(5);
            foreach (var checkListItem in defcon1CheckListItems)
            {
                checkListItems.Add(checkListItem);
            }
            foreach (var checkListItem in defcon2CheckListItems)
            {
                checkListItems.Add(checkListItem);
            }
            foreach (var checkListItem in defcon3CheckListItems)
            {
                checkListItems.Add(checkListItem);
            }
            foreach (var checkListItem in defcon4CheckListItems)
            {
                checkListItems.Add(checkListItem);
            }
            foreach (var checkListItem in defcon5CheckListItems)
            {
                checkListItems.Add(checkListItem);
            }

            return JsonConvert.SerializeObject(checkListItems);
        }
    }
}