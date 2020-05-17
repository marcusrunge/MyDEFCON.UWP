
using Checklists;
using LiveTile;
using Models;
using MyDEFCON_UWP.Helpers;
using MyDEFCON_UWP.Services;
using Services;
using Sockets;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Networking;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using static Services.StorageManagement;

namespace MyDEFCON_UWP.ViewModels
{
    public class ChecklistViewModel : Observable
    {
        private IEventService _eventService;
        private IChecklists _checkLists;
        private ILiveTile _liveTile;
        private long[] selectedItemsUnixTimeStampCreated;
        private double _gridWidth;
        private int _appDefconStatus;
        private bool _deleteInProgress;
        private ISockets _sockets;
        private CoreDispatcher _coreDispatcher;

        private int _pageDefconStatus;
        public int PageDefconStatus { get => _pageDefconStatus; set => Set(ref _pageDefconStatus, value); }

        ItemObservableCollection<CheckListItem> _defconCheckList;
        public ItemObservableCollection<CheckListItem> DefconCheckList { get { return _defconCheckList; } set { Set(ref _defconCheckList, value); } }

        ListViewSelectionMode _checklistSelectionMode;
        public ListViewSelectionMode CheckistSelectionMode { get { return _checklistSelectionMode; } set { Set(ref _checklistSelectionMode, value); } }

        List<long> _selectedItemsUnixTimeStampCreated;
        public List<long> SelectedItemsUnixTimeStampCreated { get => _selectedItemsUnixTimeStampCreated; set => Set(ref _selectedItemsUnixTimeStampCreated, value); }

        SolidColorBrush _defcon1RectangleFill;
        public SolidColorBrush Defcon1RectangleFill { get { return _defcon1RectangleFill; } set { Set(ref _defcon1RectangleFill, value); } }
        SolidColorBrush _defcon2RectangleFill;
        public SolidColorBrush Defcon2RectangleFill { get { return _defcon2RectangleFill; } set { Set(ref _defcon2RectangleFill, value); } }
        SolidColorBrush _defcon3RectangleFill;
        public SolidColorBrush Defcon3RectangleFill { get { return _defcon3RectangleFill; } set { Set(ref _defcon3RectangleFill, value); } }
        SolidColorBrush _defcon4RectangleFill;
        public SolidColorBrush Defcon4RectangleFill { get { return _defcon4RectangleFill; } set { Set(ref _defcon4RectangleFill, value); } }
        SolidColorBrush _defcon5RectangleFill;
        public SolidColorBrush Defcon5RectangleFill { get { return _defcon5RectangleFill; } set { Set(ref _defcon5RectangleFill, value); } }

        int _defcon1UnCheckedItems;
        public int Defcon1UnCheckedItems { get { return _defcon1UnCheckedItems; } set { Set(ref _defcon1UnCheckedItems, value); } }

        int _defcon2UnCheckedItems;
        public int Defcon2UnCheckedItems { get { return _defcon2UnCheckedItems; } set { Set(ref _defcon2UnCheckedItems, value); } }
        int _defcon3UnCheckedItems;
        public int Defcon3UnCheckedItems { get { return _defcon3UnCheckedItems; } set { Set(ref _defcon3UnCheckedItems, value); } }
        int _defcon4UnCheckedItems;
        public int Defcon4UnCheckedItems { get { return _defcon4UnCheckedItems; } set { Set(ref _defcon4UnCheckedItems, value); } }
        int _defcon5UnCheckedItems;
        public int Defcon5UnCheckedItems { get { return _defcon5UnCheckedItems; } set { Set(ref _defcon5UnCheckedItems, value); } }

        public ChecklistViewModel(IEventService eventService, IChecklists checkLists, ILiveTile liveTile, ISockets sockets)
        {
            _eventService = eventService;
            _checkLists = checkLists;
            _liveTile = liveTile;
            _sockets = sockets;
            _deleteInProgress = false;
            _appDefconStatus = int.Parse(GetSetting("defconStatus", "5", StorageStrategies.Roaming));
            PageDefconStatus = _appDefconStatus;
            SelectedItemsUnixTimeStampCreated = new List<long>();
            if (GetSetting<bool>("LanMulticastIsOn"))
            {
                _sockets.Datagram.IncomingMessageReceived += Datagram_IncomingMessageReceived;
                _sockets.Stream.IncomingChecklistReceived += Stream_IncomingChecklistReceived;
            }
            _coreDispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
        }

        private async void Stream_IncomingChecklistReceived(object sender, EventArgs e) => await _coreDispatcher.RunAsync(CoreDispatcherPriority.Normal, new DispatchedHandler(() => LoadedCommand.Execute(null)));

        private async void Datagram_IncomingMessageReceived(object sender, string e)
        {
            if (int.TryParse(e, out int parsedDefconStatus) && parsedDefconStatus == 0)
                await _sockets.Stream.ReceiveStringData(sender as HostName);
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>(async (param) =>
        {
            await _checkLists.Operations.SetDefconStatus(_pageDefconStatus);
            DefconCheckList = _checkLists.Collection.ActiveDefconCheckList;
            SetTextBoxWidth(_gridWidth - 52);
            _checkLists.Collection.ActiveDefconCheckList.CollectionChanged += DefconCheckList_CollectionChanged;
            _eventService.AppBarButtonClicked += AppBarButtonClicked;
            Defcon1UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.Defcon1Checklist, 1, _appDefconStatus);
            Defcon2UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.Defcon2Checklist, 2, _appDefconStatus);
            Defcon3UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.Defcon3Checklist, 3, _appDefconStatus);
            Defcon4UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.Defcon4Checklist, 4, _appDefconStatus);
            Defcon5UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.Defcon5Checklist, 5, _appDefconStatus);
            Defcon1RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.Defcon1Checklist, 1, Defcon1UnCheckedItems, _appDefconStatus);
            Defcon2RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.Defcon2Checklist, 2, Defcon2UnCheckedItems, _appDefconStatus);
            Defcon3RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.Defcon3Checklist, 3, Defcon3UnCheckedItems, _appDefconStatus);
            Defcon4RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.Defcon4Checklist, 4, Defcon4UnCheckedItems, _appDefconStatus);
            Defcon5RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.Defcon5Checklist, 5, Defcon5UnCheckedItems, _appDefconStatus);
        }));

        private async void DefconCheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (PageDefconStatus)
            {
                case 1:
                    Defcon1UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.ActiveDefconCheckList, 1, _appDefconStatus);
                    Defcon1RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.ActiveDefconCheckList, 1, Defcon1UnCheckedItems, _appDefconStatus);
                    break;
                case 2:
                    Defcon2UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.ActiveDefconCheckList, 2, _appDefconStatus);
                    Defcon2RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.ActiveDefconCheckList, 2, Defcon2UnCheckedItems, _appDefconStatus);
                    break;
                case 3:
                    Defcon3UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.ActiveDefconCheckList, 3, _appDefconStatus);
                    Defcon3RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.ActiveDefconCheckList, 3, Defcon3UnCheckedItems, _appDefconStatus);
                    break;
                case 4:
                    Defcon4UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.ActiveDefconCheckList, 4, _appDefconStatus);
                    Defcon4RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.ActiveDefconCheckList, 4, Defcon4UnCheckedItems, _appDefconStatus);
                    break;
                case 5:
                    Defcon5UnCheckedItems = UncheckedItems.Count(_checkLists.Collection.ActiveDefconCheckList, 5, _appDefconStatus);
                    Defcon5RectangleFill = UncheckedItems.RectangleFill(_checkLists.Collection.ActiveDefconCheckList, 5, Defcon5UnCheckedItems, PageDefconStatus);
                    break;
                default:
                    break;
            }
            if (!_deleteInProgress) await _checkLists.Operations.SaveCheckList(DefconCheckList, PageDefconStatus);
            UpdateTileBadge();
        }

        private async void AppBarButtonClicked(object sender, EventArgs e)
        {
            switch ((e as AppBarButtonClickedEventArgs).Button)
            {
                case "Add":
                    AddItemToChecklist();
                    break;
                case "List":
                    CheckistSelectionMode = CheckistSelectionMode == ListViewSelectionMode.Multiple ? ListViewSelectionMode.None : ListViewSelectionMode.Multiple;
                    break;
                case "Delete":
                    if (SelectedItemsUnixTimeStampCreated.Count > 0) await DeleteSelectedItems();
                    else CheckistSelectionMode = ListViewSelectionMode.None;
                    break;
                case "Sync":
                    await _sockets.Datagram.SendMessage("0");
                    break;
                default:
                    break;
            }
        }

        private async Task DeleteSelectedItems()
        {
            _deleteInProgress = true;
            selectedItemsUnixTimeStampCreated = new long[SelectedItemsUnixTimeStampCreated.Count];
            SelectedItemsUnixTimeStampCreated.CopyTo(selectedItemsUnixTimeStampCreated);
            for (int i = 0; i < selectedItemsUnixTimeStampCreated.Length; i++)
            {
                for (int j = 0; j < _checkLists.Collection.ActiveDefconCheckList.Count; j++)
                {
                    if (_checkLists.Collection.ActiveDefconCheckList[j].UnixTimeStampCreated == selectedItemsUnixTimeStampCreated[i])
                    {
                        _checkLists.Collection.ActiveDefconCheckList[j].Deleted = true;
                        _checkLists.Collection.ActiveDefconCheckList[j].Visibility = Visibility.Collapsed;
                        _checkLists.Collection.ActiveDefconCheckList[j].Checked = true;
                        _checkLists.Collection.ActiveDefconCheckList[j].UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    }
                }
            }
            CheckistSelectionMode = ListViewSelectionMode.None;
            await _checkLists.Operations.SaveCheckList(DefconCheckList, PageDefconStatus);
            _deleteInProgress = false;
        }

        private void AddItemToChecklist() => _checkLists.Collection.ActiveDefconCheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, DefconStatus = (short)PageDefconStatus, FontSize = 14, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), UnixTimeStampUpdated= DateTimeOffset.Now.ToUnixTimeMilliseconds(), Deleted = false, Visibility = Visibility.Visible, Width = _gridWidth - 52 });

        private ICommand _loadDefconChecklistCommand;
        public ICommand LoadDefconChecklistCommand => _loadDefconChecklistCommand ?? (_loadDefconChecklistCommand = new RelayCommand<object>(async (param) =>
        {
            CheckistSelectionMode = ListViewSelectionMode.None;
            _eventService.OnChecklistChanged(null);
            _checkLists.Collection.ActiveDefconCheckList.CollectionChanged -= DefconCheckList_CollectionChanged;
            PageDefconStatus = int.Parse(param as string);
            await _checkLists.Operations.SetDefconStatus(PageDefconStatus);
            DefconCheckList = _checkLists.Collection.ActiveDefconCheckList;
            SetTextBoxWidth(_gridWidth - 52);
            _checkLists.Collection.ActiveDefconCheckList.CollectionChanged += DefconCheckList_CollectionChanged;
        }));

        private ICommand _windowSizeChangedCommand;
        public ICommand WindowSizeChangedCommand => _windowSizeChangedCommand ?? (_windowSizeChangedCommand = new RelayCommand<object>((param) =>
        {
            SetTextBoxWidth(((SizeChangedEventArgs)param).NewSize.Width - 52);
            _gridWidth = ((SizeChangedEventArgs)param).NewSize.Width;
        }));

        private void SetTextBoxWidth(double value)
        {
            if (_checkLists.Collection.ActiveDefconCheckList != null) foreach (var item in _checkLists.Collection.ActiveDefconCheckList) item.Width = value;
        }

        private ICommand _unloadedCommand;
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>(async (param) =>
        {
            await _checkLists.Operations.SaveCheckList(DefconCheckList, PageDefconStatus);
        }));

        private void UpdateTileBadge()
        {
            int badgeNumber = UncheckedItemsService.CountBadgeNumber(_appDefconStatus, _checkLists.Collection.Defcon1Checklist, _checkLists.Collection.Defcon2Checklist, _checkLists.Collection.Defcon3Checklist, _checkLists.Collection.Defcon4Checklist, _checkLists.Collection.Defcon5Checklist);
            SetSetting("badgeNumber", badgeNumber.ToString(), StorageStrategies.Roaming);
            if (GetSetting<bool>("ShowUncheckedItems")) _liveTile.DefconTile.SetBadge(badgeNumber);
        }
    }
}
