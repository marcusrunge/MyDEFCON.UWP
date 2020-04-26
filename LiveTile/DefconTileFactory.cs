namespace LiveTile
{
    internal class DefconTileFactory
    {
        private static IDefconTile _defconTile;
        public static IDefconTile Create() => _defconTile ?? (_defconTile = new DefconTile());
    }
}
