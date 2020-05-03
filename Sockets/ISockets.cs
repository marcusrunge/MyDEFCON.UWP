namespace Sockets
{
    public interface ISockets
    {
        IStream Stream { get; }
        IDatagram Datagram { get; }
    }
}
