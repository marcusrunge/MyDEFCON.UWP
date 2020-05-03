namespace Sockets
{
    public class SocketsFactory
    {
        private static ISockets _sockets;
        public static ISockets Create() => _sockets ?? (_sockets = new Sockets());
    }
}
