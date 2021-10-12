using Checklists;
using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage;
using Windows.UI.Xaml;

namespace Sockets
{
    public interface IStream
    {
        event EventHandler IncomingChecklistReceived;

        Task StartListener();

        Task StopListener();

        Task SendStringData(HostName hostName, string data);

        Task<string> ReceiveStringData(HostName hostName);

        string GetJsonSerializedChecklistItems();

        Task TransferOwnership();
    }

    public class Stream : IStream
    {
        public event EventHandler IncomingChecklistReceived;

        private StreamSocketListener _streamSocketListener;

        private static IStream _stream;
        private readonly IChecklists _checklists;

        public Stream(IChecklists checklists)
        {
            _checklists = checklists;
        }

        internal static IStream Create(IChecklists checklists) => _stream ?? (_stream = new Stream(checklists));

        public async Task StartListener()
        {
            try
            {
                _streamSocketListener = new StreamSocketListener();
                _streamSocketListener.ConnectionReceived += StreamSocketListener_ConnectionReceived;

                await _streamSocketListener.BindServiceNameAsync("4537");
            }
            catch (Exception ex)
            {
                SocketErrorStatus webErrorStatus = SocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        private async void StreamSocketListener_ConnectionReceived(StreamSocketListener sender, StreamSocketListenerConnectionReceivedEventArgs args)
        {
            using (var streamWriter = new StreamWriter(args.Socket.OutputStream.AsStreamForWrite()))
            {
                await streamWriter.WriteLineAsync(GetJsonSerializedChecklistItems());
                await streamWriter.FlushAsync();
            }
        }

        public async Task StopListener()
        {
            _streamSocketListener.ConnectionReceived -= StreamSocketListener_ConnectionReceived;
            await _streamSocketListener.CancelIOAsync();
            _streamSocketListener = null;
        }

        public async Task SendStringData(HostName hostName, string data)
        {
            try
            {
                using (var streamSocket = new StreamSocket())
                {
                    await streamSocket.ConnectAsync(hostName, "4537");
                    using (System.IO.Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
                    {
                        using (var streamWriter = new StreamWriter(outputStream))
                        {
                            await streamWriter.WriteLineAsync(data);
                            await streamWriter.FlushAsync();
                        }
                    }
                    await streamSocket.CancelIOAsync();
                    streamSocket.Dispose();
                }
            }
            catch (Exception ex)
            {
                _ = SocketError.GetStatus(ex.GetBaseException().HResult);
            }
        }

        public async Task<string> ReceiveStringData(HostName hostName)
        {
            double textBoxWidth = 256;
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("textBoxWidth")) textBoxWidth = (double)localSettings.Values["textBoxWidth"];

            string response = String.Empty;
            try
            {
                using (var streamSocket = new StreamSocket())
                {
                    await streamSocket.ConnectAsync(hostName, "4537");

                    using (System.IO.Stream inputStream = streamSocket.InputStream.AsStreamForRead())
                    {
                        using (StreamReader streamReader = new StreamReader(inputStream))
                        {
                            response = await streamReader.ReadLineAsync();
                            var checkListItems = JsonConvert.DeserializeObject<List<CheckListItem>>(response);
                            var defcon1CheckListItems = _checklists.Collection.Defcon1Checklist;
                            var defcon2CheckListItems = _checklists.Collection.Defcon2Checklist;
                            var defcon3CheckListItems = _checklists.Collection.Defcon3Checklist;
                            var defcon4CheckListItems = _checklists.Collection.Defcon4Checklist;
                            var defcon5CheckListItems = _checklists.Collection.Defcon5Checklist;
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
                                    if (!itemFound)
                                    {
                                        item.Width = textBoxWidth;
                                        defcon1CheckListItems.Add(item);
                                    }
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
                                    if (!itemFound)
                                    {
                                        item.Width = textBoxWidth;
                                        defcon2CheckListItems.Add(item);
                                    }
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
                                    if (!itemFound)
                                    {
                                        item.Width = textBoxWidth;
                                        defcon3CheckListItems.Add(item);
                                    }
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
                                    if (!itemFound)
                                    {
                                        item.Width = textBoxWidth;
                                        defcon4CheckListItems.Add(item);
                                    }
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
                                    if (!itemFound)
                                    {
                                        item.Width = textBoxWidth;
                                        defcon5CheckListItems.Add(item);
                                    }
                                }
                            }
                            await _checklists.Operations.SaveCheckList(defcon1CheckListItems, 1);
                            await _checklists.Operations.SaveCheckList(defcon2CheckListItems, 2);
                            await _checklists.Operations.SaveCheckList(defcon3CheckListItems, 3);
                            await _checklists.Operations.SaveCheckList(defcon4CheckListItems, 4);
                            await _checklists.Operations.SaveCheckList(defcon5CheckListItems, 5);
                        }
                    }
                    await streamSocket.CancelIOAsync();
                    streamSocket.Dispose();
                }
            }
            catch (Exception ex)
            {
                _ = SocketError.GetStatus(ex.GetBaseException().HResult);
            }
            OnIncomingChecklistReceived();
            return response;
        }

        private void OnIncomingChecklistReceived() => IncomingChecklistReceived?.Invoke(this, new EventArgs());

        public string GetJsonSerializedChecklistItems()
        {
            var checkListItems = new List<CheckListItem>();
            var defcon1CheckListItems = _checklists.Collection.Defcon1Checklist;
            var defcon2CheckListItems = _checklists.Collection.Defcon2Checklist;
            var defcon3CheckListItems = _checklists.Collection.Defcon3Checklist;
            var defcon4CheckListItems = _checklists.Collection.Defcon4Checklist;
            var defcon5CheckListItems = _checklists.Collection.Defcon5Checklist;
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

        public async Task TransferOwnership()
        {
            await _streamSocketListener.CancelIOAsync();
            _streamSocketListener.TransferOwnership("myDefconStreamSocket");
        }
    }
}