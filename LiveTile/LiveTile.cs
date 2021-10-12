namespace LiveTile
{
    public interface ILiveTile
    {
        IDefconTile DefconTile { get; }
    }

    internal class LiveTile : DefconTileBase
    {
        internal LiveTile()
        {
            _defconTile = global::LiveTile.DefconTile.Create(this);
        }
    }
}