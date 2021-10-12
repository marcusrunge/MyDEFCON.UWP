namespace LiveTile
{
    public interface ILiveTileFactory
    {
        ILiveTile Create();
    }

    public class LiveTileFactory : ILiveTileFactory
    {
        private static ILiveTile _liveTile;

        public ILiveTile Create() => _liveTile ?? (_liveTile = new LiveTile());
    }
}