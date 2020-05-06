using BackgroundTask;
using MyDEFCON_UWP.Helpers;
using Services;
using Sockets;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Xaml;
using static Services.StorageManagement;

namespace MyDEFCON_UWP.ViewModels
{
    public class SettingsPivotViewModel : Observable
    {
        private int _defconStatus;
        private ISockets _sockets;

        bool _useTransparentTile = default;
        public bool UseTransparentTile { get { return _useTransparentTile; } set { Set(ref _useTransparentTile, value); } }

        bool _showUncheckedItems = default;
        public bool ShowUncheckedItems { get { return _showUncheckedItems; } set { Set(ref _showUncheckedItems, value); } }

        bool _backgroundTask = default;
        public bool BackgroundTask { get { return _backgroundTask; } set { Set(ref _backgroundTask, value); } }

        bool _lanBroadcastIsOn = default;
        public bool LanBroadcastIsOn { get { return _lanBroadcastIsOn; } set { Set(ref _lanBroadcastIsOn, value); } }

        bool _lanMulticastIsOn = default;
        public bool LanMulticastIsOn { get { return _lanMulticastIsOn; } set { Set(ref _lanMulticastIsOn, value); } }

        List<string> _intervall = default;
        public List<string> Intervall { get { return _intervall; } set { Set(ref _intervall, value); } }

        int _selectedTimeIntervallIndex = default;
        public int SelectedTimeIntervallIndex { get { return _selectedTimeIntervallIndex; } set { Set(ref _selectedTimeIntervallIndex, value); } }

        Visibility _iotVisibility = default;
        public Visibility IotVisibility { get { return _iotVisibility; } set { Set(ref _iotVisibility, value); } }

        public SettingsPivotViewModel(ISockets sockets)
        {
            _sockets = sockets;
            Intervall = new List<string> { "15min", "30min", "1hour", "3hours", "6hours", "12hours", "daily" };
            UseTransparentTile = GetSetting<bool>("UseTransparentTile");
            ShowUncheckedItems = GetSetting<bool>("ShowUncheckedItems");
            BackgroundTask = GetSetting<bool>("BackgroundTask");
            LanBroadcastIsOn = GetSetting<bool>("LanBroadcastIsOn");
            LanMulticastIsOn = GetSetting<bool>("LanMulticastIsOn");
            BackgroundTask = GetSetting<bool>("BackgroundTask");
            SelectedTimeIntervallIndex = GetSetting<int>("SelectedTimeIntervallIndex");
            IotVisibility = AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.IoT") ? Visibility.Visible : Visibility.Collapsed;
            _defconStatus = int.Parse(GetSetting("defconStatus", "5", StorageStrategies.Roaming));
            PropertyChanged += SettingsPivotViewModel_PropertyChanged;
        }

        private async void SettingsPivotViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UseTransparentTile":
                    SetSetting(e.PropertyName, UseTransparentTile);
                    LiveTileManagement.SetLiveTile(_defconStatus, UseTransparentTile);
                    break;
                case "ShowUncheckedItems":
                    SetSetting(e.PropertyName, ShowUncheckedItems);
                    if (ShowUncheckedItems)
                    {
                        int badgeNumber = Convert.ToInt16(GetSetting<string>("badgeNumber", location: StorageStrategies.Roaming));
                        LiveTileManagement.UpdateTileBadge(badgeNumber);
                    }
                    break;
                case "BackgroundTask":
                    SetSetting(e.PropertyName, BackgroundTask);
                    if (BackgroundTask) await BackgroundTaskManagement.Register<TileUpdateBackgroundTask>(new TimeTrigger(IntervallInMinutes(), false));
                    else await BackgroundTaskManagement.Unregister<BroadcastListenerBackgroundTask>();
                    break;
                case "LanBroadcastIsOn":
                    SetSetting(e.PropertyName, LanBroadcastIsOn);
                    if (LanBroadcastIsOn) await _sockets.Datagram.StartListener();
                    else await _sockets.Datagram.StopListener();
                    break;
                case "LanMulticastIsOn":
                    SetSetting(e.PropertyName, LanMulticastIsOn);
                    if (LanMulticastIsOn) await _sockets.Stream.StartListener();
                    else await _sockets.Stream.StopListener();
                    break;
                case "SelectedTimeIntervallIndex":
                    SetSetting(e.PropertyName, SelectedTimeIntervallIndex);
                    break;
                default:
                    break;
            }
        }

        private ICommand _removeBackgroundTasksCommand;
        public ICommand RemoveBackgroundTasksCommand => _removeBackgroundTasksCommand ?? (_removeBackgroundTasksCommand = new RelayCommand<object>(async (param) =>
        {
            await BackgroundTaskManagement.UnregisterAll();
            BackgroundTask = false;
            LanBroadcastIsOn = false;
            LanMulticastIsOn = false;
        }));

        private ICommand _restartCommand;
        public ICommand RestartCommand => _restartCommand ?? (_restartCommand = new RelayCommand<object>((param) =>
        {
            ShutdownManager.BeginShutdown(ShutdownKind.Restart, TimeSpan.FromSeconds(0));
        }));

        private ICommand _shutdownCommand;
        public ICommand ShutdownCommand => _shutdownCommand ?? (_shutdownCommand = new RelayCommand<object>((param) =>
        {
            ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0));
        }));

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
    }
}
