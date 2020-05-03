using BackgroundLibrary;
using BackgroundTask;
using System;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace Sockets
{
    public interface IDatagram
    {
        Task StartListener();
        Task SendMessage(string message);
        event EventHandler<string> IncomingMessageReceived;
        Task TransferOwnership();
    }
    internal class Datagram : IDatagram
    {
        public string IncomingMessage { get; set; }
        public HostName RemoteAddress { get; set; }
        public event EventHandler<string> IncomingMessageReceived;
        private DatagramSocket datagramSocket = null;
        private bool _isOrigin = default(bool);

        private static IDatagram _datagram;
        internal static IDatagram Create() => _datagram ?? (_datagram = new Datagram());

        public async Task StartListener()
        {
            //ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            var window = CoreWindow.GetForCurrentThread();
            var dispatcher = window.Dispatcher;
            var backgroundTaskRegistration = await BackgroundTaskService.Register<BroadcastListenerBackgroundTask>(new SocketActivityTrigger());
            datagramSocket = new DatagramSocket();
            datagramSocket.EnableTransferOwnership(backgroundTaskRegistration.TaskId, SocketActivityConnectedStandbyAction.DoNotWake);
            await datagramSocket.BindServiceNameAsync("4536");
            datagramSocket.MessageReceived += async (s, e) =>
            {
                _isOrigin = false;
                if (!_isOrigin)
                {
                    try
                    {
                        RemoteAddress = e.RemoteAddress;
                        uint stringLength = e.GetDataReader().UnconsumedBufferLength;
                        IncomingMessage = e.GetDataReader().ReadString(stringLength);
                        //if(int.TryParse(IncomingMessage, out int parsedDefconStatus) && parsedDefconStatus > 0 && parsedDefconStatus < 6) roamingSettings.Values["defconStatus"] = IncomingMessage;
                        await dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => OnIncomingMessageReceived(IncomingMessage)));
                    }
                    catch (Exception) { }
                }
                _isOrigin = false;
            };
        }

        public async Task SendMessage(string message)
        {
            try
            {
                IOutputStream outputStream;
                HostName hostname = new HostName("255.255.255.255");
                outputStream = await datagramSocket.GetOutputStreamAsync(hostname, "4536");
                DataWriter dataWriter = new DataWriter(outputStream);
                dataWriter.WriteString(message);
                _isOrigin = true;
                await dataWriter.StoreAsync();
            }
            catch (Exception) { }
        }

        private void OnIncomingMessageReceived(string s) => IncomingMessageReceived?.Invoke(this, s);

        public async Task TransferOwnership()
        {
            await datagramSocket?.CancelIOAsync();
            datagramSocket?.TransferOwnership("myDefconDatagramSocket");
        }
    }
}
