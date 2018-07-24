namespace Services
{
    //public sealed class DatagramService
    //{
    //    public string IncomingMessage { get; set; }
    //    public event EventHandler<string> IncomingMessageReceived;
    //    private DatagramSocket datagramSocket = null;
    //    public IAsyncAction StartListener()
    //    {
    //        var window = CoreWindow.GetForCurrentThread();
    //        var dispatcher = window.Dispatcher;
    //        return Task.Run(async () =>
    //        {
    //            //--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//
    //            /*var streamSocketListener = new Windows.Networking.Sockets.StreamSocketListener();
    //            streamSocketListener.ConnectionReceived += async (s, e) =>
    //            {
    //                using (var streamReader = new StreamReader(e.Socket.InputStream.AsStreamForRead()))
    //                {
    //                    IncomingMessage = await streamReader.ReadLineAsync();
    //                }                    
    //                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => OnIncomingMessageReceived(IncomingMessage)));
    //                s.Dispose();                    
    //            };
    //            await streamSocketListener.BindServiceNameAsync("4536");*/

    //            //--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//
    //            datagramSocket = new DatagramSocket();
    //            datagramSocket.MessageReceived += async (s, e) =>
    //            {
    //                uint stringLength = e.GetDataReader().UnconsumedBufferLength;
    //                IncomingMessage = e.GetDataReader().ReadString(stringLength);
    //                await dispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => OnIncomingMessageReceived(IncomingMessage)));
    //            };
    //            await datagramSocket.BindServiceNameAsync("4537");
    //        }).AsAsyncAction();

    //    }

    //    public IAsyncAction SendMessage(string message)
    //    {
    //        return Task.Run(async () =>
    //        {
    //            //--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//--Unicast--//
    //            /*using (var streamSocket = new Windows.Networking.Sockets.StreamSocket())
    //            {
    //                var hostName = new HostName("localhost");
    //                await streamSocket.ConnectAsync(hostName, "4536");
    //                using (Stream outputStream = streamSocket.OutputStream.AsStreamForWrite())
    //                {
    //                    using (var streamWriter = new StreamWriter(outputStream))
    //                    {
    //                        await streamWriter.WriteLineAsync(message);
    //                        await streamWriter.FlushAsync();
    //                    }
    //                }
    //            }*/

    //            //--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//--Broadcast--//
    //            IOutputStream outputStream;
    //            HostName hostname = new HostName("255.255.255.255");
    //            outputStream = await datagramSocket.GetOutputStreamAsync(hostname, "4536");
    //            DataWriter dataWriter = new DataWriter(outputStream);
    //            dataWriter.WriteString(message);
    //            await dataWriter.StoreAsync();
    //        }).AsAsyncAction();
    //    }

    //    private void OnIncomingMessageReceived(string s) => IncomingMessageReceived?.Invoke(this, s);

    //    public void Dispose()
    //    {
    //        datagramSocket?.Dispose();
    //    }
    //}
}