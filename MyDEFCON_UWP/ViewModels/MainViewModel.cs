
using MyDEFCON_UWP.Helpers;
using Services;
using System;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP.ViewModels
{
    public class MainViewModel : Observable
    {
        private IEventService _eventService;       

        private int _defconStatus;
        public int DefconStatus { get => _defconStatus; set => Set(ref _defconStatus, value); }

        public MainViewModel(IEventService eventService)
        {
            _eventService = eventService;
            DefconStatus = int.Parse(StorageService.GetSetting("defconStatus", "5", StorageService.StorageStrategies.Roaming));
        }

        private ICommand _setDefconStatusCommand;
        public ICommand SetDefconStatusCommand => _setDefconStatusCommand ?? (_setDefconStatusCommand = new RelayCommand<object>((param) =>
        {
            StorageService.SetSetting("defconStatus", (string)param, StorageService.StorageStrategies.Roaming);
            DefconStatus = int.Parse(param as string);
        }));

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>((param) =>
        {   
            DataTransferManager.GetForCurrentView().DataRequested += MainViewModel_DataRequested;
            _eventService.AppBarButtonClicked += (s, e) =>
            {
                if((e as AppBarButtonClickedEventArgs).Button.Equals("Share")) DataTransferManager.ShowShareUI();
            };
        }));

        private void MainViewModel_DataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dataPackage = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            dataPackage.Properties.Title = "DEFCON STATUS";
            dataPackage.Properties.Description = "DEFCON Status Payload for sharing";            
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///ShareImages/Defcon{0}.png", _defconStatus.ToString()))));
            deferral.Complete();
        }

        private ICommand _unloadedCommand;
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>((param) =>
        {
            DataTransferManager.GetForCurrentView().DataRequested -= MainViewModel_DataRequested;
        }));
    }
}
