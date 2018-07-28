using BackgroundLibrary;
using BackgroundTask;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.UI.Xaml.Navigation;

namespace MyDEFCON_UWP.ViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();
        public override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            SettingsPartViewModel.LoadSettingsFromLocalSettings();
            SettingsPartViewModel.LoadDefconStatusFromRoamingSettings();

            return base.OnNavigatedToAsync(parameter, mode, state);
        }
    }

    public class SettingsPartViewModel : ViewModelBase
    {
        #region Fields
        bool _useTransparentTile = default(bool);
        bool _showUncheckedItems = default(bool);
        bool _backgroundTask = default(bool);
        bool _lanBroadcastIsOn = default(bool);
        bool _lanMulticastIsOn = default(bool);
        List<string> _intervall = default(List<string>);
        bool _pageInitialize = default(bool);
        int _selectedTimeIntervallIndex = default(int);
        Windows.Storage.ApplicationDataContainer localSettings;
        int _defconStatus;
        #endregion

        #region Properties
        public bool UseTransparentTile { get { return _useTransparentTile; } set { Set(ref _useTransparentTile, value); SaveTransparentTileSetting(); } }
        public bool ShowUncheckedItems { get { return _showUncheckedItems; } set { Set(ref _showUncheckedItems, value); SaveShowUncheckedItemsSetting(); } }
        public bool BackgroundTask { get { return _backgroundTask; } set { Set(ref _backgroundTask, value); } }
        public bool LanBroadcastIsOn { get { return _lanBroadcastIsOn; } set { Set(ref _lanBroadcastIsOn, value); } }
        public bool LanMulticastIsOn { get { return _lanMulticastIsOn; } set { Set(ref _lanMulticastIsOn, value); } }
        public List<string> Intervall { get { return _intervall; } set { Set(ref _intervall, value); } }
        public int SelectedTimeIntervallIndex { get { return _selectedTimeIntervallIndex; } set { Set(ref _selectedTimeIntervallIndex, value); UpdateTimeIntervall(); } }
        #endregion

        #region Constructor
        public SettingsPartViewModel()
        {
            Intervall = new List<string> { "15min", "30min", "1hour", "3hours", "6hours", "12hours", "daily" };
            _pageInitialize = true;
            localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            PropertyChanged += async (s, e) =>
            {
                if (e.PropertyName.Equals("BackgroundTask")) await SaveBackgroundTaskSetting();
                else if (e.PropertyName.Equals("LanBroadcastIsOn")) SaveLanBroadcastIsOnSetting();
                else if (e.PropertyName.Equals("LanMulticastIsOn")) SaveLanMulticastIsOnSetting();
            };
        }
        #endregion

        #region Methods        
        public void LoadSettingsFromLocalSettings()
        {
            if (localSettings.Values.ContainsKey("useTransparentTile")) UseTransparentTile = (bool)localSettings.Values["useTransparentTile"];
            if (localSettings.Values.ContainsKey("showUncheckedItems")) ShowUncheckedItems = (bool)localSettings.Values["showUncheckedItems"];
            if (localSettings.Values.ContainsKey("backgroundTask")) BackgroundTask = (bool)localSettings.Values["backgroundTask"];
            if (localSettings.Values.ContainsKey("lanBroadcastIsOn")) LanBroadcastIsOn = (bool)localSettings.Values["lanBroadcastIsOn"];
            if (localSettings.Values.ContainsKey("lanMulticastIsOn")) LanMulticastIsOn = (bool)localSettings.Values["lanMulticastIsOn"];
            if (localSettings.Values.ContainsKey("selectedTimeIntervallIndex")) SelectedTimeIntervallIndex = (int)localSettings.Values["selectedTimeIntervallIndex"];
            _pageInitialize = false;
        }

        public void LoadDefconStatusFromRoamingSettings()
        {
            Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("defconStatus")) _defconStatus = Convert.ToInt16(roamingSettings.Values["defconStatus"].ToString());
            else _defconStatus = 5;
        }

        private void SaveTransparentTileSetting()
        {
            if (!_pageInitialize)
            {
                localSettings.Values["useTransparentTile"] = UseTransparentTile;
                LiveTileService.SetLiveTile(_defconStatus, _useTransparentTile);
            }
        }

        private void SaveShowUncheckedItemsSetting()
        {
            if (!_pageInitialize) localSettings.Values["showUncheckedItems"] = ShowUncheckedItems;
            if (ShowUncheckedItems)
            {
                int badgeNumber = 0;
                Windows.Storage.ApplicationDataContainer roamingSettings = Windows.Storage.ApplicationData.Current.RoamingSettings;
                if (roamingSettings.Values.ContainsKey("badgeNumber")) badgeNumber = Convert.ToInt16(roamingSettings.Values["badgeNumber"].ToString());
                LiveTileService.UpdateTileBadge(badgeNumber);
            }
        }

        private async Task SaveBackgroundTaskSetting()
        {
            if (!_pageInitialize)
            {
                localSettings.Values["backgroundTask"] = BackgroundTask;
                if (BackgroundTask) await BackgroundTaskService.Register<TileUpdateBackgroundTask>(new TimeTrigger(IntervallInMinutes(), false));
                else await BackgroundTaskService.Unregister<BroadcastListenerBackgroundTask>();
            }
        }

        private void SaveLanBroadcastIsOnSetting()
        {
            if (!_pageInitialize) localSettings.Values["lanBroadcastIsOn"] = LanBroadcastIsOn;
        }

        private void SaveLanMulticastIsOnSetting()
        {
            if (!_pageInitialize) localSettings.Values["lanMulticastIsOn"] = LanBroadcastIsOn;
        }

        private async void UpdateTimeIntervall()
        {
            if (!_pageInitialize)
            {
                localSettings.Values["selectedTimeIntervallIndex"] = SelectedTimeIntervallIndex;
                if (BackgroundTask) await BackgroundTaskService.Register<TileUpdateBackgroundTask>(new TimeTrigger(IntervallInMinutes(), false));
            }
        }

        private uint IntervallInMinutes()
        {
            switch (SelectedTimeIntervallIndex)
            {
                case 0:
                    return 15;
                case 1:
                    return 30;
                case 2:
                    return 60;
                case 3:
                    return 180;
                case 4:
                    return 360;
                case 5:
                    return 720;
                case 6:
                    return 1440;
                default:
                    break;
            }
            return 15;
        }
        #endregion

        #region Commands
        private DelegateCommand _removeBackgroundTasksCommand;
        public DelegateCommand RemoveBackgroundTasksCommand
        {
            get
            {
                if (_removeBackgroundTasksCommand != null) return _removeBackgroundTasksCommand;
                _removeBackgroundTasksCommand = new DelegateCommand(async () =>
                {
                    await BackgroundTaskService.UnregisterAll();
                    BackgroundTask = false;
                    LanBroadcastIsOn = false;
                });
                return _removeBackgroundTasksCommand;
            }
        }

        DelegateCommand _shutdownCommand;
        public DelegateCommand ShutdownCommand
            => _shutdownCommand ?? (_shutdownCommand = new DelegateCommand(() =>
            {
                ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0));
            }, () => true));

        DelegateCommand _restartCommand;
        public DelegateCommand RestartCommand
            => _restartCommand ?? (_restartCommand = new DelegateCommand(() =>
            {
                ShutdownManager.BeginShutdown(ShutdownKind.Restart, TimeSpan.FromSeconds(0));
            }, () => true));
        #endregion
    }

    public class AboutPartViewModel : ViewModelBase
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor
        #endregion

        #region Methods

        #endregion

        #region Commands
        private DelegateCommand _rateCommand;
        public DelegateCommand RateCommand
        {
            get
            {
                if (_rateCommand != null)
                    return _rateCommand;
                _rateCommand = new DelegateCommand
                    (
                        async () => await Launcher.LaunchUriAsync(new Uri("ms-windows-store:reviewapp?appid=d6facdb7-724c-47dc-91d5-489d5acb2eb2"))
                    );
                return _rateCommand;
            }
        }

        private DelegateCommand _emailCommand;
        public DelegateCommand EmailCommand
        {
            get
            {
                if (_emailCommand != null)
                    return _emailCommand;
                _emailCommand = new DelegateCommand
                    (
                        async () => await Launcher.LaunchUriAsync(new Uri("mailto:code_m@outlook.de?subject=MyDEFCON App"))
                    );
                return _emailCommand;
            }
        }
        #endregion
    }
}