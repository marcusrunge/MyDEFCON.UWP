
using Models;
using MyDEFCON_UWP.Helpers;
using MyDEFCON_UWP.Services;
using Services;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Streams;

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
            DefconStatus = int.Parse(StorageManagement.GetSetting("defconStatus", "5", StorageManagement.StorageStrategies.Roaming));
        }

        private ICommand _setDefconStatusCommand;
        public ICommand SetDefconStatusCommand => _setDefconStatusCommand ?? (_setDefconStatusCommand = new RelayCommand<object>(async (param) =>
        {
            StorageManagement.SetSetting("defconStatus", (string)param, StorageManagement.StorageStrategies.Roaming);
            DefconStatus = int.Parse(param as string);
            LiveTileManagement.SetLiveTile(DefconStatus, StorageManagement.GetSetting<bool>("UseTransparentTile"));
            await ReverseUncheck(DefconStatus);
            await UpdateTileBadge();
        }));

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>((param) =>
        {
            DataTransferManager.GetForCurrentView().DataRequested += MainViewModel_DataRequested;
            _eventService.AppBarButtonClicked += (s, e) =>
            {
                if ((e as AppBarButtonClickedEventArgs).Button.Equals("Share")) DataTransferManager.ShowShareUI();
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

        private async Task UpdateTileBadge()
        {
            var defcon1CheckList = await CheckListManagement.LoadCheckList(1);
            var defcon2CheckList = await CheckListManagement.LoadCheckList(2);
            var defcon3CheckList = await CheckListManagement.LoadCheckList(3);
            var defcon4CheckList = await CheckListManagement.LoadCheckList(4);
            var defcon5CheckList = await CheckListManagement.LoadCheckList(5);
            int badgeNumber = UncheckedItemsService.CountBadgeNumber(DefconStatus, defcon1CheckList, defcon2CheckList, defcon3CheckList, defcon4CheckList, defcon5CheckList);
            StorageManagement.SetSetting("badgeNumber", badgeNumber.ToString(), StorageManagement.StorageStrategies.Roaming);
            if (StorageManagement.GetSetting<bool>("ShowUncheckedItems")) LiveTileManagement.UpdateTileBadge(badgeNumber);
        }

        private async Task ReverseUncheck(int defconStatus)
        {
            var defcon1CheckList = await CheckListManagement.LoadCheckList(1);
            var defcon2CheckList = await CheckListManagement.LoadCheckList(2);
            var defcon3CheckList = await CheckListManagement.LoadCheckList(3);
            var defcon4CheckList = await CheckListManagement.LoadCheckList(4);
            switch (defconStatus)
            {
                case 2:
                    if (_defconStatus != 2)
                    {
                        defcon1CheckList = UncheckCollection(defcon1CheckList);
                        await CheckListManagement.SaveCheckList(defcon1CheckList, 1);
                    }

                    break;
                case 3:
                    if (_defconStatus != 3)
                    {
                        defcon1CheckList = UncheckCollection(defcon1CheckList);
                        defcon2CheckList = UncheckCollection(defcon2CheckList);
                        await CheckListManagement.SaveCheckList(defcon1CheckList, 1);
                        await CheckListManagement.SaveCheckList(defcon2CheckList, 2);
                    }

                    break;
                case 4:
                    if (_defconStatus != 4)
                    {
                        defcon1CheckList = UncheckCollection(defcon1CheckList);
                        defcon2CheckList = UncheckCollection(defcon2CheckList);
                        defcon3CheckList = UncheckCollection(defcon3CheckList);
                        await CheckListManagement.SaveCheckList(defcon1CheckList, 1);
                        await CheckListManagement.SaveCheckList(defcon2CheckList, 2);
                        await CheckListManagement.SaveCheckList(defcon3CheckList, 3);
                    }

                    break;
                case 5:
                    if (_defconStatus != 5)
                    {
                        defcon1CheckList = UncheckCollection(defcon1CheckList);
                        defcon2CheckList = UncheckCollection(defcon2CheckList);
                        defcon3CheckList = UncheckCollection(defcon3CheckList);
                        defcon4CheckList = UncheckCollection(defcon4CheckList);
                        await CheckListManagement.SaveCheckList(defcon1CheckList, 1);
                        await CheckListManagement.SaveCheckList(defcon2CheckList, 2);
                        await CheckListManagement.SaveCheckList(defcon3CheckList, 3);
                        await CheckListManagement.SaveCheckList(defcon4CheckList, 4);
                    }
                    break;
                default:
                    break;
            }
        }

        private ItemObservableCollection<CheckListItem> UncheckCollection(ItemObservableCollection<CheckListItem> collection)
        {
            foreach (var item in collection) item.Checked = false;
            return collection;
        }
    }
}
