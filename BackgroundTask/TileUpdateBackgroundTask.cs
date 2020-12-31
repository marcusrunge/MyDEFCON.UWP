using CommonServiceLocator;
using LiveTile;
using System;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class TileUpdateBackgroundTask : IBackgroundTask
    {
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            ILiveTileFactory liveTileFactory;
            try
            {
                liveTileFactory = ServiceLocator.Current.GetInstance<ILiveTileFactory>();
            }
            catch (Exception)
            {
                liveTileFactory = new LiveTileFactory();
            }
            if (liveTileFactory != null)
            {
                var liveTile = liveTileFactory.Create();
                var backgroundWorkCost = BackgroundWorkCost.CurrentBackgroundWorkCost;
                if (backgroundWorkCost == BackgroundWorkCostValue.High)
                {
                    return;
                }
                else
                {
                    var deferral = taskInstance.GetDeferral();
                    liveTile.DefconTile.SetTile(LoadDefconStatusFromRoamingSettings());
                    liveTile.DefconTile.SetBadge(BadgeNumber());
                    deferral.Complete();
                }
            }
        }

        public int LoadDefconStatusFromRoamingSettings()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("defconStatus")) return Convert.ToInt16(roamingSettings.Values["defconStatus"].ToString());
            else return 5;
        }

        private bool LoadUseTransparentTileSetting()
        {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("useTransparentTile")) return (bool)localSettings.Values["useTransparentTile"];
            else return false;
        }

        private int BadgeNumber()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("badgeNumber")) return Convert.ToInt16(roamingSettings.Values["badgeNumber"]);
            else return 0;
        }
    }
}