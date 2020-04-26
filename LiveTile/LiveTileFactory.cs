namespace LiveTile
{
    public static class LiveTileFactory
    {
        private static ILiveTile _liveTile;
        public static ILiveTile Create() => _liveTile ?? (_liveTile = new LiveTile());
    }
}
