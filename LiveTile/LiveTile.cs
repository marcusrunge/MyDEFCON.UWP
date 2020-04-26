namespace LiveTile
{
    public interface ILiveTile
    {
        IDefconTile DefconTile { get; }
    }
    public class LiveTile : ILiveTile
    {
        public IDefconTile DefconTile => DefconTileFactory.Create();
    }
}
