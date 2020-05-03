namespace Sockets
{
    internal class Sockets : SocketsBase
    {
        internal Sockets()
        {
            _datagram = global::Sockets.Datagram.Create();
            _stream = global::Sockets.Stream.Create();
        }
    }
}
