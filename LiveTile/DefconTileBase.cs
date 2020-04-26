using System.Xml.Linq;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace LiveTile
{
    internal abstract class DefconTileBase
    {
        private protected XmlDocument CreateTiles(IDefconImagePaths defconImagePaths)
        {
            XDocument xDocument = new XDocument(
                new XElement("tile", new XAttribute("version", 3),
                    new XElement("visual",
                        // Small Tile  
                        new XElement("binding", new XAttribute("template", "TileSmall"),
                            new XElement("image", new XAttribute("src", defconImagePaths.Small), new XAttribute("placement", "background"))
                        ),

                        //Medium Tile
                        new XElement("binding", new XAttribute("template", "TileMedium"),
                                new XElement("image", new XAttribute("src", defconImagePaths.Medium), new XAttribute("placement", "background"))
                                ),

                        // Wide Tile  
                        new XElement("binding", new XAttribute("template", "TileWide"),
                            new XElement("image", new XAttribute("src", defconImagePaths.Wide), new XAttribute("placement", "background"))
                        ),

                        //Large Tile  
                        new XElement("binding", new XAttribute("template", "TileLarge"),
                            new XElement("image", new XAttribute("src", defconImagePaths.Large), new XAttribute("placement", "background"))
                        )
                    )
                )
            );
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xDocument.ToString());
            return xmlDocument;
        }

        private protected bool LoadShowUncheckedItemsSetting() => ApplicationData.Current.LocalSettings.Values.ContainsKey("showUncheckedItems") ? (bool)ApplicationData.Current.LocalSettings.Values["showUncheckedItems"] : false;
    }
}
