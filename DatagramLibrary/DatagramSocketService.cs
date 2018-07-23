using BackgroundLibrary;
using BackgroundTask;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Networking;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.UI.Core;

namespace SocketLibrary
{
    public class DatagramSocketService
    {
        public string IncomingMessage { get; set; }
        public event EventHandler<string> IncomingMessageReceived;
        private DatagramSocket datagramSocket = null;
        public async Task StartListener()
        {
            var window = CoreWindow.GetForCurrentThread();
            var dispatcher = window.Dispatcher;
            var backgroundTaskRegistration = await BackgroundTaskService.Register<BroadcastListenerBackgroundTask>(new SocketActivityTrigger());
            datagramSocket = new DatagramSocket();
            datagramSocket.EnableTransferOwnership(backgroundTaskRegistration.TaskId, SocketActivityConnectedStandbyAction.DoNotWake);
            await datagramSocket.BindServiceNameAsync("4536");
            datagramSocket.MessageReceived += async (s, e) =>
            {
                uint stringLength = e.GetDataReader().UnconsumedBufferLength;
                IncomingMessage = e.GetDataReader().ReadString(stringLength);
                if(int.TryParse(IncomingMessage, out int parsedValue) && parsedValue == 0) await dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(async () => await HandleStreamSocketConnection(e.RemoteAddress)));
                else await dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => OnIncomingMessageReceived(IncomingMessage)));
            };
        }

        private async Task HandleStreamSocketConnection(HostName remoteHostName)
        {
            var streamSocketService = new StreamSocketService();
        }

        public async Task SendMessage(string message)
        {
            IOutputStream outputStream;
            HostName hostname = new HostName("255.255.255.255");
            outputStream = await datagramSocket.GetOutputStreamAsync(hostname, "4536");
            DataWriter dataWriter = new DataWriter(outputStream);
            dataWriter.WriteString(message);
            await dataWriter.StoreAsync();
        }

        private void OnIncomingMessageReceived(string s) => IncomingMessageReceived?.Invoke(this, s);

        public async Task Dispose()
        {
            await datagramSocket?.CancelIOAsync();
            datagramSocket?.TransferOwnership("myDefconSocket");
            //datagramSocket?.Dispose();
        }
    }
}
