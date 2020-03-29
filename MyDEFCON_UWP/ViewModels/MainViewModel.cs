
using MyDEFCON_UWP.Helpers;
using Services;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP.ViewModels
{
    public class MainViewModel : Observable
    {
        private double _fontSize;
        public double FontSize { get => _fontSize; set => Set(ref _fontSize, value); }

        private int _defconStatus;
        public int DefconStatus { get => _defconStatus; set => Set(ref _defconStatus, value); }

        public MainViewModel()
        {
            FontSize = 14;
        }

        private ICommand _setFontSizeCommand;
        public ICommand SetFontSizeCommand => _setFontSizeCommand ?? (_setFontSizeCommand = new RelayCommand<object>((param) =>
        {
            double calculatedTextBlockWidth = (param as SizeChangedEventArgs).NewSize.Height * 0.5 * 6.5;
            if ((param as SizeChangedEventArgs).NewSize.Width > calculatedTextBlockWidth) FontSize = Math.Floor((param as SizeChangedEventArgs).NewSize.Height * 0.5);
            else FontSize = Math.Floor((param as SizeChangedEventArgs).NewSize.Width / 6.5);
        }));

        private ICommand _setDefconStatusCommand;
        public ICommand SetDefconStatusCommand => _setDefconStatusCommand ?? (_setDefconStatusCommand = new RelayCommand<object>((param) =>
        {
            StorageService.SetSetting("defconStatus", (string)param, StorageService.StorageStrategies.Roaming);
            DefconStatus = int.Parse(param as string);
        }));

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>((param) =>
        {            
            DefconStatus = int.Parse(StorageService.GetSetting("defconStatus", "5", StorageService.StorageStrategies.Roaming));
        }));
    }
}
