
using Checklists;
using LiveTile;
using MyDEFCON_UWP.Core.Eventaggregator;
using MyDEFCON_UWP.Helpers;
using MyDEFCON_UWP.Services;
using Sockets;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;
using Windows.UI.Core;
using static Services.StorageManagement;

namespace MyDEFCON_UWP.ViewModels
{
    public class MainViewModel : Observable
    {
        private IEventAggregator _eventAggregator;
        private IChecklists _checkLists;
        private ILiveTile _liveTile;
        private ISockets _sockets;
        private CoreDispatcher _coreDispatcher;

        private int _defconStatus;
        public int DefconStatus { get => _defconStatus; set => Set(ref _defconStatus, value); }

        public MainViewModel(IEventAggregator eventAggregator, IChecklists checkLists, ILiveTile liveTile, ISockets sockets)
        {
            _eventAggregator = eventAggregator;
            _checkLists = checkLists;
            _liveTile = liveTile;
            _sockets = sockets;
            DefconStatus = int.Parse(GetSetting("defconStatus", "5", StorageStrategies.Roaming));
            if (GetSetting<bool>("LanBroadcastIsOn")) _sockets.Datagram.IncomingMessageReceived += Datagram_IncomingMessageReceived;
            _coreDispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            ApplicationDataChanged += async (s, e) =>
            {
                DefconStatus=int.Parse((string)s.RoamingSettings.Values["defconStatus"]);
                await UpdateDefconStatus();
            };
        }

        private async void Datagram_IncomingMessageReceived(object sender, string e)
        {
            await _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() =>
            {
                if (int.TryParse(e, out int parsedDefconStatus) && parsedDefconStatus > 0 && parsedDefconStatus < 6) DefconStatus = parsedDefconStatus;
            }));
        }

        private ICommand _setDefconStatusCommand;
        public ICommand SetDefconStatusCommand => _setDefconStatusCommand ?? (_setDefconStatusCommand = new RelayCommand<object>(async (param) =>
        {
            SetSetting("defconStatus", (string)param, StorageStrategies.Roaming);
            DefconStatus = int.Parse(param as string);
            await UpdateDefconStatus();
        }));

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>((param) =>
        {
            DataTransferManager.GetForCurrentView().DataRequested += MainViewModel_DataRequested;
            _eventAggregator.Subscribe.AppBarButtonClicked += (s, e) =>
            {
                if (e.Button.Equals("Share")) DataTransferManager.ShowShareUI();
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

        private void UpdateTileBadge()
        {            
            int badgeNumber = UncheckedItemsService.CountBadgeNumber(DefconStatus, _checkLists.Collection.Defcon1Checklist, _checkLists.Collection.Defcon2Checklist, _checkLists.Collection.Defcon3Checklist, _checkLists.Collection.Defcon4Checklist, _checkLists.Collection.Defcon5Checklist);
            SetSetting("badgeNumber", badgeNumber.ToString(), StorageStrategies.Roaming);
            if (GetSetting<bool>("ShowUncheckedItems")) _liveTile.DefconTile.SetBadge(badgeNumber);
        }

        private async Task UpdateDefconStatus()
        {            
            _liveTile.DefconTile.SetTile(DefconStatus);
            await _checkLists.Operations.ReverseUncheck(DefconStatus);
            UpdateTileBadge();
            if (GetSetting<bool>("LanBroadcastIsOn")) await _sockets.Datagram.SendMessage(DefconStatus.ToString());
        }
    }
}
