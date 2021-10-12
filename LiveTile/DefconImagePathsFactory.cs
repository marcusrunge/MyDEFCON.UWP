namespace LiveTile
{
    internal static class DefconImagePathsFactory
    {
        internal static IDefconImagePaths Create(int status) => new DefconImagePaths(status);
    }
}