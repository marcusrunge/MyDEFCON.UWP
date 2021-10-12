using Windows.Storage;

namespace LiveTile
{
    internal interface IDefconImagePaths
    {
        string Small { get; }
        string Medium { get; }
        string Wide { get; }
        string Large { get; }
    }

    internal class DefconImagePaths : IDefconImagePaths
    {
        private readonly int _status = 5;
        private readonly bool _transparent = false;

        public string Small => _transparent ? string.Format("TransparentTileImages/Defcon{0}SmallTile.png", _status.ToString()) : string.Format("TileImages/Defcon{0}SmallTile.png", _status.ToString());

        public string Medium => _transparent ? string.Format("TransparentTileImages/Defcon{0}MediumTile.png", _status.ToString()) : string.Format("TileImages/Defcon{0}MediumTile.png", _status.ToString());

        public string Wide => _transparent ? string.Format("TransparentTileImages/Defcon{0}WideTile.png", _status.ToString()) : string.Format("TileImages/Defcon{0}WideTile.png", _status.ToString());

        public string Large => _transparent ? string.Format("TransparentTileImages/Defcon{0}LargeTile.png", _status.ToString()) : string.Format("TileImages/Defcon{0}LargeTile.png", _status.ToString());

        internal DefconImagePaths(int status)
        {
            _status = status;
            _transparent = ApplicationData.Current.LocalSettings.Values.ContainsKey("useTransparentTile") && (bool)ApplicationData.Current.LocalSettings.Values["useTransparentTile"];
        }
    }
}