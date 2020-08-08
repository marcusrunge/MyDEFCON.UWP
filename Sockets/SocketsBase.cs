using Checklists;

namespace Sockets
{
    internal abstract class SocketsBase : ISockets
    {
        protected IChecklists _checklists;
        protected IStream _stream;
        public IStream Stream => _stream;
        protected IDatagram _datagram;
        public IDatagram Datagram => _datagram;

        public SocketsBase(IChecklists checklists) => _checklists = checklists;
    }
}
