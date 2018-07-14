using DatagramLibrary;
using MyDEFCON_UWP.Services.SettingsServices;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Mvvm;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

namespace MyDEFCON_UWP.ViewModels
{
    public class FullScreenPageViewModel : ViewModelBase
    {
        DatagramService _datagramService;
        UIElement _uIElement;
        I2cDevice _i2CDevice;
        double _onPointerPressedY, _onPointerReleasedY;
        bool _useTransparentTile = default(bool);
        bool _isFullScreen = default(bool);
        int _defconStatus;
        public int DefconStatus { get { return _defconStatus; } set { Set(ref _defconStatus, value); } }
        double _fontSize;
        public double FontSize { get { return _fontSize; } set { Set(ref _fontSize, value); } }
        VisualState _defconVisualState;
        public VisualState DefconVisualState { get { return _defconVisualState; } set { Set(ref _defconVisualState, value); } }

        public override async Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            SettingsService.Instance.IsFullScreen = true;
            SettingsService.Instance.ShowHamburgerButton = false;
            _isFullScreen = true;
            await LoadDefconStatusFromRoamingSettings();
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("useTransparentTile")) _useTransparentTile = (bool)localSettings.Values["useTransparentTile"];
            else _useTransparentTile = false;
            ApplicationData.Current.DataChanged += async (s, e) =>
            {
                await LoadDefconStatusFromRoamingSettings();
                LiveTileService.SetLiveTile(_defconStatus, _useTransparentTile);
            };

            if (localSettings.Values.ContainsKey("lanBroadcastIsOn") && (bool)localSettings.Values["lanBroadcastIsOn"])
            {
                _datagramService = new DatagramService();
                await _datagramService.StartListener();
                _datagramService.IncomingMessageReceived += async (s, e) =>
                {
                    int.TryParse(e, out int defconStatus);
                    if (defconStatus > 0 && defconStatus < 6)
                    {
                        DefconStatus = defconStatus;
                        ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                        roamingSettings.Values["defconStatus"] = e;
                        await LoadDefconStatusFromRoamingSettings();
                        LiveTileService.SetLiveTile(_defconStatus, _useTransparentTile);
                    }
                };
            }

            //Raspi ONLY
            string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            I2cConnectionSettings i2CConnectionSettings = new I2cConnectionSettings(0x45);
            IReadOnlyList<DeviceInformation> deviceInformationCollection = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            if (deviceInformationCollection.Count > 0)
            {
                var i2CDevice = await I2cDevice.FromIdAsync(deviceInformationCollection[0].Id, i2CConnectionSettings);
                _i2CDevice = i2CDevice;
            }
        }

        private async Task<object> LoadDefconStatusFromRoamingSettings()
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("defconStatus"))
            {
                await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                DefconStatus = Convert.ToInt16(roamingSettings.Values["defconStatus"].ToString()));
            }
            else
                DefconStatus = 5;
            switch (DefconStatus)
            {
                case 1:
                    DefconVisualState = VisualState.Defcon1VisualState;
                    break;
                case 2:
                    DefconVisualState = VisualState.Defcon2VisualState;
                    break;
                case 3:
                    DefconVisualState = VisualState.Defcon3VisualState;
                    break;
                case 4:
                    DefconVisualState = VisualState.Defcon4VisualState;
                    break;
                case 5:
                    DefconVisualState = VisualState.Defcon5VisualState;
                    break;
                default:
                    break;
            }
            return Task.FromResult<object>(null);
        }

        public void border_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double calculatedTextBlockWidth = e.NewSize.Height * 0.75 * 6.5;
            if (e.NewSize.Width > calculatedTextBlockWidth)
            {
                FontSize = Math.Floor(e.NewSize.Height * 0.75);
            }
            else
            {
                FontSize = Math.Floor(e.NewSize.Width / 6.5);
            }
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if (_datagramService != null) await _datagramService.Dispose();
            SettingsService.Instance.IsFullScreen = false;
            SettingsService.Instance.ShowHamburgerButton = true;
            _isFullScreen = false;
        }

        DelegateCommand _fullScreenCommand;
        public DelegateCommand FullScreenCommand
            => _fullScreenCommand ?? (_fullScreenCommand = new DelegateCommand(() =>
            {
                if (_isFullScreen)
                {
                    SettingsService.Instance.IsFullScreen = false;
                    SettingsService.Instance.ShowHamburgerButton = true;
                    _isFullScreen = false;
                }
                else
                {
                    SettingsService.Instance.IsFullScreen = true;
                    SettingsService.Instance.ShowHamburgerButton = false;
                    _isFullScreen = true;
                }
            }, () => true));

        DelegateCommand<PointerRoutedEventArgs> _onPointerPressedCommand;
        public DelegateCommand<PointerRoutedEventArgs> OnPointerPressedCommand
            => _onPointerPressedCommand ?? (_onPointerPressedCommand = new DelegateCommand<PointerRoutedEventArgs>((e) =>
            {
                _uIElement = e.OriginalSource as UIElement;
                _onPointerPressedY = e.GetCurrentPoint(_uIElement).Position.Y;
            }, (e) => true));

        DelegateCommand<PointerRoutedEventArgs> _onPointerReleasedCommand;
        public DelegateCommand<PointerRoutedEventArgs> OnPointerReleasedCommand
            => _onPointerReleasedCommand ?? (_onPointerReleasedCommand = new DelegateCommand<PointerRoutedEventArgs>((e) =>
            {
                _onPointerReleasedY = e.GetCurrentPoint(_uIElement).Position.Y;
                double deltaY = _onPointerReleasedY - _onPointerPressedY;
                if (deltaY > 0 && deltaY > 10) ToggleScreenBacklight(false);
                if (deltaY < 0 && deltaY < -10) ToggleScreenBacklight(true);
            }, (e) => true));

        private void ToggleScreenBacklight(bool isScreenBacklightOn)
        {
            byte brightness = isScreenBacklightOn ? (byte)255 : (byte)7;
            try
            {
                _i2CDevice?.Write(new byte[] { 0x86, brightness });
            }
            catch (Exception) { }
        }
    }
}