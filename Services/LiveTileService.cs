using System.Xml.Linq;
using Windows.UI.Notifications;

namespace Services
{
    internal class ImagePath
    {
        public string Small { get; set; }
        public string Medium { get; set; }
        public string Wide { get; set; }
        public string Large { get; set; }
        public ImagePath(int status, bool transparent)
        {
            if (transparent)
            {
                Small = string.Format("TransparentTileImages/Defcon{0}SmallTile.png", status.ToString());
                Medium = string.Format("TransparentTileImages/Defcon{0}MediumTile.png", status.ToString());
                Wide = string.Format("TransparentTileImages/Defcon{0}WideTile.png", status.ToString());
                Large = string.Format("TransparentTileImages/Defcon{0}LargeTile.png", status.ToString());
            }
            else
            {
                Small = string.Format("TileImages/Defcon{0}SmallTile.png", status.ToString());
                Medium = string.Format("TileImages/Defcon{0}MediumTile.png", status.ToString());
                Wide = string.Format("TileImages/Defcon{0}WideTile.png", status.ToString());
                Large = string.Format("TileImages/Defcon{0}LargeTile.png", status.ToString());
            }
        }
    }
    public static class LiveTileService
    {
        public static void SetLiveTile(int status, bool transparent)
        {
            var xmlDocument = CreateTiles(new ImagePath(status, transparent));
            var tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            var tileNotification = new TileNotification(xmlDocument);
            tileUpdater.Update(tileNotification);
        }

        static Windows.Data.Xml.Dom.XmlDocument CreateTiles(ImagePath imagePath)
        {
            XDocument xDocument = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // Small Tile  
                        new XElement("binding", new XAttribute("template", "TileSmall"),
                            new XElement("image", new XAttribute("src", imagePath.Small), new XAttribute("placement", "background"))
                        ),

                        //Medium Tile
                        new XElement("binding", new XAttribute("template", "TileMedium"),
                                new XElement("image", new XAttribute("src", imagePath.Medium), new XAttribute("placement", "background"))
                                ),

                        // Wide Tile  
                        new XElement("binding", new XAttribute("template", "TileWide"),
                            new XElement("image", new XAttribute("src", imagePath.Wide), new XAttribute("placement", "background"))
                        ),

                        //Large Tile  
                        new XElement("binding", new XAttribute("template", "TileLarge"),
                            new XElement("image", new XAttribute("src", imagePath.Large), new XAttribute("placement", "background"))
                        )
                    )
                )
            );
            Windows.Data.Xml.Dom.XmlDocument xmlDocument = new Windows.Data.Xml.Dom.XmlDocument();
            xmlDocument.LoadXml(xDocument.ToString());
            return xmlDocument;
        }

        private static bool LoadShowUncheckedItemsSetting()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("showUncheckedItems")) return (bool)localSettings.Values["showUncheckedItems"];
            else return false;
        }

        public static void UpdateTileBadge(int number)
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