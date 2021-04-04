using Checklists;

namespace Sockets
{
    public interface ISocketsFactory
    {
        ISockets Create();
    }

    public class SocketsFactory : ISocketsFactory
    {
        private readonly IChecklists _checklists;
        public SocketsFactory(IChecklists checklists) => _checklists = checklists;
        private static ISockets _sockets;
        public ISockets Create() => _sockets ?? (_sockets = new Sockets(_checklists));
    }
}
