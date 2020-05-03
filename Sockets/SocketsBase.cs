namespace Sockets
{
    internal abstract class SocketsBase : ISockets
    {
        protected IStream _stream;
        public IStream Stream => _stream;
        protected IDatagram _datagram;
        public IDatagram Datagram => _datagram;
    }
}
