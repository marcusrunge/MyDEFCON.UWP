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
        Task StopListener();
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

        private static IDatagram _datagram;
        internal static IDatagram Create() => _datagram ?? (_datagram = new Datagram());

        public async Task StartListener()
        {
            var backgroundTaskRegistration = await BackgroundTaskService.Register<BroadcastListenerBackgroundTask>(new SocketActivityTrigger());
            datagramSocket = new DatagramSocket();
            datagramSocket.EnableTransferOwnership(backgroundTaskRegistration.TaskId, SocketActivityConnectedStandbyAction.DoNotWake);
            await datagramSocket.BindServiceNameAsync("4536");
            datagramSocket.MessageReceived += DatagramSocket_MessageReceived;
        }

        public async Task StopListener()
        {
            datagramSocket.MessageReceived -= DatagramSocket_MessageReceived;
            await datagramSocket.CancelIOAsync();
            datagramSocket = null;
            await BackgroundTaskService.Unregister<BroadcastListenerBackgroundTask>();
        }

        private void DatagramSocket_MessageReceived(DatagramSocket sender, DatagramSocketMessageReceivedEventArgs args)
        {            
            if (sender.Information.LocalPort!=args.RemotePort)
            {
                try
                {
                    RemoteAddress = args.RemoteAddress;
                    uint stringLength = args.GetDataReader().UnconsumedBufferLength;
                    IncomingMessage = args.GetDataReader().ReadString(stringLength);
                    OnIncomingMessageReceived(args, IncomingMessage);
                }
                catch (Exception) { }
            }
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
                await dataWriter.StoreAsync();
            }
            catch (Exception) { }
        }

        private void OnIncomingMessageReceived(DatagramSocketMessageReceivedEventArgs a, string s) => IncomingMessageReceived?.Invoke(a.RemoteAddress, s);

        public async Task TransferOwnership()
        {
            await datagramSocket?.CancelIOAsync();
            datagramSocket?.TransferOwnership("myDefconDatagramSocket");
        }
    }
}
