using Windows.UI.Notifications;

namespace LiveTile
{
    public interface IDefconTile
    {
        void SetBadge(int number);
        void Set(int status);
    }
    internal class DefconTile : DefconTileBase, IDefconTile
    {
        public void Set(int status)
        {
            var xmlDocument = CreateTiles(DefconImagePathsFactory.Create(status));
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            var tileNotification = new TileNotification(xmlDocument);
            tileUpdater.Update(tileNotification);
        }

        public void SetBadge(int number)
        {
            if (LoadShowUncheckedItemsSetting())
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