using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
