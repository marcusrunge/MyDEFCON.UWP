using LiveTile;
using System;
using Unity;
using Windows.ApplicationModel.Background;

namespace BackgroundTask
{
    public sealed class TileUpdateBackgroundTask : IBackgroundTask
    {
        private IUnityContainer _container;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                _container.RegisterType<ILiveTileFactory, LiveTileFactory>();
                _container.RegisterFactory<ILiveTile>((c) => c.Resolve<ILiveTileFactory>().Create(), FactoryLifetime.Singleton);
            }
            var liveTile = _container.Resolve<ILiveTile>();
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