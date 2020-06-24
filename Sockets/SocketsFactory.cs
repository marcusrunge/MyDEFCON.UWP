namespace Sockets
{
    public class SocketsFactory
    {
        private static ISockets _sockets;
        public static ISockets Create(/*IChecklists checklists*/) => _sockets ?? (_sockets = new Sockets(/*checklists*/));
    }
}
