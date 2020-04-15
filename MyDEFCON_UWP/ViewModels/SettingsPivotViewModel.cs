using MyDEFCON_UWP.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP.ViewModels
{
    public class SettingsPivotViewModel : Observable
    {
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

        private ICommand _removeBackgroundTasksCommand;
        public ICommand RemoveBackgroundTasksCommand => _removeBackgroundTasksCommand ?? (_removeBackgroundTasksCommand = new RelayCommand<object>((param) =>
        {
            
        }));

        private ICommand _restartCommand;
        public ICommand RestartCommand => _restartCommand ?? (_restartCommand = new RelayCommand<object>((param) =>
        {

        }));

        private ICommand _shutdownCommand;
        public ICommand ShutdownCommand => _shutdownCommand ?? (_shutdownCommand = new RelayCommand<object>((param) =>
        {

        }));
    }
}
