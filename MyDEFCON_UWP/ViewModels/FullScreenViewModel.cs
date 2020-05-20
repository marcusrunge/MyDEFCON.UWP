using MyDEFCON_UWP.Helpers;
using Sockets;
using System;
using System.Windows.Input;
using Windows.Devices.I2c;
using Windows.UI.Core;
using Windows.UI.Xaml;
using static Services.StorageManagement;

namespace MyDEFCON_UWP.ViewModels
{
    public class FullScreenViewModel : Observable
    {
        private UIElement _uIElement;
        private I2cDevice _i2CDevice;
        private ISockets _sockets;
        private CoreDispatcher _coreDispatcher;

        private int _defconStatus;
        public int DefconStatus { get => _defconStatus; set => Set(ref _defconStatus, value); }

        public FullScreenViewModel(ISockets sockets)
        {
            _sockets = sockets;
            if (GetSetting<bool>("LanBroadcastIsOn")) _sockets.Datagram.IncomingMessageReceived += Datagram_IncomingMessageReceived;
            _coreDispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            ApplicationDataChanged += (s, e) =>
            {
                DefconStatus = int.Parse((string)s.RoamingSettings.Values["defconStatus"]);
            };
        }

        private async void Datagram_IncomingMessageReceived(object sender, string e)
        {
            await _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                if (int.TryParse(e, out int parsedDefconStatus) && parsedDefconStatus > 0 && parsedDefconStatus < 6) DefconStatus = parsedDefconStatus;
            }));
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>((param) =>
        {
            
        }));

        private ICommand _unloadedCommand;
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>((param) =>
        {
            
        }));

        private ICommand _fullScreenCommand;
        public ICommand FullScreenCommand => _fullScreenCommand ?? (_fullScreenCommand = new RelayCommand<object>((param) =>
        {
            
        }));

        private ICommand _onPointerPressedCommand;
        public ICommand OnPointerPressedCommand => _onPointerPressedCommand ?? (_onPointerPressedCommand = new RelayCommand<object>((param) =>
        {

        }));

        private ICommand _onPointerReleasedCommand;
        public ICommand OnPointerReleasedCommand => _onPointerReleasedCommand ?? (_onPointerReleasedCommand = new RelayCommand<object>((param) =>
        {

        }));
    }
}
