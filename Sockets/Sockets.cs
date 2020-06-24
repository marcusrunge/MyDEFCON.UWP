using Checklists;

namespace Sockets
{
    internal class Sockets : SocketsBase
    {
        internal Sockets(/*IChecklists checklists*/) : base(/*checklists*/)
        {
            _datagram = global::Sockets.Datagram.Create();
            _stream = global::Sockets.Stream.Create(_checklists);
        }
    }
}
