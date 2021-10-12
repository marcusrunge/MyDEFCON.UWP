using BackgroundTask;
using LiveTile;
using MyDEFCON_UWP.Helpers;
using Sockets;
using Storage;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Windows.ApplicationModel.Background;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP.ViewModels
{
    public class SettingsPivotViewModel : Observable
    {
        private readonly int _defconStatus;
        private readonly ISockets _sockets;
        private readonly ILiveTile _liveTile;
        private readonly IStorage _storage;

        private bool _useTransparentTile = default;
        public bool UseTransparentTile { get { return _useTransparentTile; } set { Set(ref _useTransparentTile, value); } }

        private bool _showUncheckedItems = default;
        public bool ShowUncheckedItems { get { return _showUncheckedItems; } set { Set(ref _showUncheckedItems, value); } }

        private bool _backgroundTask = default;
        public bool BackgroundTask { get { return _backgroundTask; } set { Set(ref _backgroundTask, value); } }

        private bool _lanBroadcastIsOn = default;
        public bool LanBroadcastIsOn { get { return _lanBroadcastIsOn; } set { Set(ref _lanBroadcastIsOn, value); } }

        private bool _lanMulticastIsOn = default;
        public bool LanMulticastIsOn { get { return _lanMulticastIsOn; } set { Set(ref _lanMulticastIsOn, value); } }

        private List<string> _intervall = default;
        public List<string> Intervall { get { return _intervall; } set { Set(ref _intervall, value); } }

        private int _selectedTimeIntervallIndex = default;
        public int SelectedTimeIntervallIndex { get { return _selectedTimeIntervallIndex; } set { Set(ref _selectedTimeIntervallIndex, value); } }

        private Visibility _iotVisibility = default;
        public Visibility IotVisibility { get { return _iotVisibility; } set { Set(ref _iotVisibility, value); } }

        public SettingsPivotViewModel(ISockets sockets, ILiveTile liveTile, IStorage storage)
        {
            _sockets = sockets;
            _liveTile = liveTile;
            _storage = storage;
            Intervall = new List<string> { "15min", "30min", "1hour", "3hours", "6hours", "12hours", "daily" };
            UseTransparentTile = _storage.Setting.GetSetting<bool>("UseTransparentTile");
            ShowUncheckedItems = _storage.Setting.GetSetting<bool>("ShowUncheckedItems");
            BackgroundTask = _storage.Setting.GetSetting<bool>("BackgroundTask");
            LanBroadcastIsOn = _storage.Setting.GetSetting<bool>("LanBroadcastIsOn");
            LanMulticastIsOn = _storage.Setting.GetSetting<bool>("LanMulticastIsOn");
            BackgroundTask = _storage.Setting.GetSetting<bool>("BackgroundTask");
            SelectedTimeIntervallIndex = _storage.Setting.GetSetting<int>("SelectedTimeIntervallIndex");
            IotVisibility = AnalyticsInfo.VersionInfo.DeviceFamily.Equals("Windows.IoT") ? Visibility.Visible : Visibility.Collapsed;
            _defconStatus = int.Parse(_storage.Setting.GetSetting("defconStatus", "5", StorageStrategies.Roaming));
            PropertyChanged += SettingsPivotViewModel_PropertyChanged;
        }

        private async void SettingsPivotViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "UseTransparentTile":
                    _storage.Setting.SetSetting(e.PropertyName, UseTransparentTile);
                    _liveTile.DefconTile.SetTile(_defconStatus);
                    break;

                case "ShowUncheckedItems":
                    _storage.Setting.SetSetting(e.PropertyName, ShowUncheckedItems);
                    if (ShowUncheckedItems)
                    {
                        int badgeNumber = Convert.ToInt16(_storage.Setting.GetSetting<string>("badgeNumber", location: StorageStrategies.Roaming));
                        _liveTile.DefconTile.SetBadge(badgeNumber);
                    }
                    break;

                case "BackgroundTask":
                    _storage.Setting.SetSetting(e.PropertyName, BackgroundTask);
                    if (BackgroundTask) await BackgroundTaskManagement.Register<TileUpdateBackgroundTask>(new TimeTrigger(IntervallInMinutes(), false));
                    else await BackgroundTaskManagement.Unregister<BroadcastListenerBackgroundTask>();
                    break;

                case "LanBroadcastIsOn":
                    _storage.Setting.SetSetting(e.PropertyName, LanBroadcastIsOn);
                    if (LanBroadcastIsOn) await _sockets.Datagram.StartListener();
                    else await _sockets.Datagram.StopListener();
                    break;

                case "LanMulticastIsOn":
                    _storage.Setting.SetSetting(e.PropertyName, LanMulticastIsOn);
                    if (LanMulticastIsOn) await _sockets.Stream.StartListener();
                    else await _sockets.Stream.StopListener();
                    break;

                case "SelectedTimeIntervallIndex":
                    _storage.Setting.SetSetting(e.PropertyName, SelectedTimeIntervallIndex);
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

        private ICommand _unloadedCommand;

        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>((param) =>
        {
            PropertyChanged -= SettingsPivotViewModel_PropertyChanged;
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