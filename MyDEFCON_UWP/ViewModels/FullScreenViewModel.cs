﻿using Microsoft.UI.Xaml.Controls;
using MyDEFCON_UWP.Helpers;
using Services;
using Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Devices.Enumeration;
using Windows.Devices.I2c;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using static Services.StorageManagement;

namespace MyDEFCON_UWP.ViewModels
{
    public class FullScreenViewModel : Observable
    {
        private UIElement _uIElement;
        private I2cDevice _i2CDevice;
        private ISockets _sockets;
        private IEventService _eventService;
        private CoreDispatcher _coreDispatcher;
        double _onPointerPressedY, _onPointerReleasedY;
        bool _isFullScreen = default(bool);

        private string _defconVisualState;
        public string DefconVisualState { get => _defconVisualState; set => Set(ref _defconVisualState, value); }
                
        public FullScreenViewModel(ISockets sockets, IEventService eventService)
        {
            _sockets = sockets;
            _eventService = eventService;
            if (GetSetting<bool>("LanBroadcastIsOn")) _sockets.Datagram.IncomingMessageReceived += Datagram_IncomingMessageReceived;
            _coreDispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            ApplicationDataChanged += async (s, e) =>
            {
                await SetDefconVisualState(int.Parse((string)s.RoamingSettings.Values["defconStatus"]));
            };
        }

        private async void Datagram_IncomingMessageReceived(object sender, string e)
        {
            if (int.TryParse(e, out int parsedDefconStatus) && parsedDefconStatus > 0 && parsedDefconStatus < 6) await SetDefconVisualState(parsedDefconStatus);
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>(async (param) =>
        {
            await SetDefconVisualState(int.Parse(GetSetting("defconStatus", "5", StorageStrategies.Roaming)));
            _isFullScreen = true;            
            string i2cDeviceSelector = I2cDevice.GetDeviceSelector();
            I2cConnectionSettings i2CConnectionSettings = new I2cConnectionSettings(0x45);
            IReadOnlyList<DeviceInformation> deviceInformationCollection = await DeviceInformation.FindAllAsync(i2cDeviceSelector);
            if (deviceInformationCollection.Count > 0)
            {
                var i2CDevice = await I2cDevice.FromIdAsync(deviceInformationCollection[0].Id, i2CConnectionSettings);
                _i2CDevice = i2CDevice;
            }
            _eventService.OnPaneDisplayModeChangeChanged(new PaneDisplayModeChangedEventArgs(4));
        }));

        private ICommand _unloadedCommand;
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>((param) =>
        {
            _eventService.OnPaneDisplayModeChangeChanged(new PaneDisplayModeChangedEventArgs(3));
        }));

        private ICommand _fullScreenCommand;
        public ICommand FullScreenCommand => _fullScreenCommand ?? (_fullScreenCommand = new RelayCommand<object>((param) =>
        {
            if (_isFullScreen)
            {
                _eventService.OnPaneDisplayModeChangeChanged(new PaneDisplayModeChangedEventArgs(3));
                _isFullScreen = false;
            }
            else
            {
                _eventService.OnPaneDisplayModeChangeChanged(new PaneDisplayModeChangedEventArgs(4));
                _isFullScreen = true;
            }
        }));

        private ICommand _onPointerPressedCommand;
        public ICommand OnPointerPressedCommand => _onPointerPressedCommand ?? (_onPointerPressedCommand = new RelayCommand<object>((param) =>
        {
            _uIElement = ((PointerRoutedEventArgs)param).OriginalSource as UIElement;
            _onPointerPressedY = ((PointerRoutedEventArgs)param).GetCurrentPoint(_uIElement).Position.Y;
        }));

        private ICommand _onPointerReleasedCommand;
        public ICommand OnPointerReleasedCommand => _onPointerReleasedCommand ?? (_onPointerReleasedCommand = new RelayCommand<object>((param) =>
        {
            _onPointerReleasedY = ((PointerRoutedEventArgs)param).GetCurrentPoint(_uIElement).Position.Y;
            double deltaY = _onPointerReleasedY - _onPointerPressedY;
            if (deltaY > 0 && deltaY > 10) ToggleScreenBacklight(false);
            if (deltaY < 0 && deltaY < -10) ToggleScreenBacklight(true);
        }));

        private void ToggleScreenBacklight(bool isScreenBacklightOn)
        {
            byte brightness = isScreenBacklightOn ? (byte)255 : (byte)7;
            try
            {
                _i2CDevice?.Write(new byte[] { 0x86, brightness });
            }
            catch (Exception) { }
        }

        private async Task SetDefconVisualState(int status)
        {
            await _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                switch (status)
                {
                    case 1:
                        DefconVisualState = "Defcon1VisualState";
                        break;
                    case 2:
                        DefconVisualState = "Defcon2VisualState";
                        break;
                    case 3:
                        DefconVisualState = "Defcon3VisualState";
                        break;
                    case 4:
                        DefconVisualState = "Defcon4VisualState";
                        break;
                    default:
                        DefconVisualState = "Defcon5VisualState";
                        break;
                }
            }));
        }
    }
}
