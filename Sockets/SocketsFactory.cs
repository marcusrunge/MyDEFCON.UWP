using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sockets
{    
    public class SocketsFactory
    {
        private static ISockets _sockets;
        public static ISockets Create() => _sockets ?? (_sockets = new Sockets());
    }
}
