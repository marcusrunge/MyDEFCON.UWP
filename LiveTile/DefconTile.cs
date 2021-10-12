using Windows.UI.Notifications;

namespace LiveTile
{
    public interface IDefconTile
    {
        void SetBadge(int number);

        void SetTile(int status);
    }

    internal class DefconTile : IDefconTile
    {
        private readonly DefconTileBase _defconTileBase;

        private static IDefconTile _defconTile;

        public static IDefconTile Create(DefconTileBase defconTileBase) => _defconTile ?? (_defconTile = new DefconTile(defconTileBase));

        internal DefconTile(DefconTileBase defconTileBase) => _defconTileBase = defconTileBase;

        public void SetTile(int status)
        {
            var xmlDocument = _defconTileBase.CreateTiles(DefconImagePathsFactory.Create(status));
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            var tileNotification = new TileNotification(xmlDocument);
            tileUpdater.Update(tileNotification);
        }

        public void SetBadge(int number)
        {
            if (_defconTileBase.LoadShowUncheckedItemsSetting())
            {
                //Build Badge
                var type = BadgeTemplateType.BadgeNumber;
                var xml = BadgeUpdateManager.GetTemplateContent(type);
                //Update Element
                var elements = xml.GetElementsByTagName("badge");
                var element = elements[0] as Windows.Data.Xml.Dom.XmlElement;
                element.SetAttribute("value", number.ToString());
                //Send to lockscreen
                var badgeUpdater = BadgeUpdateManager.CreateBadgeUpdaterForApplication();
                var notification = new BadgeNotification(xml);
                badgeUpdater.Update(notification);
            }
            else BadgeUpdateManager.CreateBadgeUpdaterForApplication().Clear();
        }
    }
}