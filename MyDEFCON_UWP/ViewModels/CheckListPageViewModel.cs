using Template10.Mvvm;
using System.Collections.Generic;
using System.Threading.Tasks;
using Template10.Services.NavigationService;
using Windows.UI.Xaml.Navigation;
using Windows.UI.ViewManagement;
using Windows.UI;
using MyDEFCON_UWP.Services;
using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Core;
using Windows.Storage;
using SocketLibrary;
using Services;
using System.Linq;
using Models;

namespace MyDEFCON_UWP.ViewModels
{
    public enum VisualState { Defcon0VisualState, Defcon1VisualState, Defcon2VisualState, Defcon3VisualState, Defcon4VisualState, Defcon5VisualState }

    public class CheckListPageViewModel : ViewModelBase
    {
        #region Fields
        bool _noUpdate, _isSender;
        ItemObservableCollection<CheckListItem> _defcon1CheckList;
        ItemObservableCollection<CheckListItem> _defcon2CheckList;
        ItemObservableCollection<CheckListItem> _defcon3CheckList;
        ItemObservableCollection<CheckListItem> _defcon4CheckList;
        ItemObservableCollection<CheckListItem> _defcon5CheckList;
        int _defconStatus;
        VisualState _defconVisualState;
        double _screenWidth;
        double _textBoxWidth;
        double _scaleFactor;
        double _fontSize;
        Visibility _addIconVisibility;
        //Visibility _cancelIconVisibility;
        Visibility _deleteIconVisibility;
        ListViewSelectionMode _defconListViewSelectionMode;
        List<CheckListItem> selectedItems;
        List<CheckListItem> _defcon1selectedItems;
        List<CheckListItem> _defcon2selectedItems;
        List<CheckListItem> _defcon3selectedItems;
        List<CheckListItem> _defcon4selectedItems;
        List<CheckListItem> _defcon5selectedItems;
        object _objectItem;
        SolidColorBrush _defcon1CheckRectangleFill;
        SolidColorBrush _defcon2CheckRectangleFill;
        SolidColorBrush _defcon3CheckRectangleFill;
        SolidColorBrush _defcon4CheckRectangleFill;
        SolidColorBrush _defcon5CheckRectangleFill;
        int _defcon1UnCheckedItems;
        int _defcon2UnCheckedItems;
        int _defcon3UnCheckedItems;
        int _defcon4UnCheckedItems;
        int _defcon5UnCheckedItems;
        string _textBox;
        private bool selectionModeEnabled;
        bool _useTransparentTile = default(bool);
        DatagramSocketService _datagramService;
        StreamSocketService _streamSocketService;
        #endregion

        #region Properties
        public string TextBox { get { return _textBox; } set { Set(ref _textBox, value); } }
        public ItemObservableCollection<CheckListItem> Defcon1CheckList { get { return _defcon1CheckList; } set { Set(ref _defcon1CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon2CheckList { get { return _defcon2CheckList; } set { Set(ref _defcon2CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon3CheckList { get { return _defcon3CheckList; } set { Set(ref _defcon3CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon4CheckList { get { return _defcon4CheckList; } set { Set(ref _defcon4CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon5CheckList { get { return _defcon5CheckList; } set { Set(ref _defcon5CheckList, value); } }
        public List<CheckListItem> Defcon1SelectedItem { get { return _defcon1selectedItems; } set { Set(ref _defcon1selectedItems, value); } }
        public List<CheckListItem> Defcon2SelectedItem { get { return _defcon2selectedItems; } set { Set(ref _defcon2selectedItems, value); } }
        public List<CheckListItem> Defcon3SelectedItem { get { return _defcon3selectedItems; } set { Set(ref _defcon3selectedItems, value); } }
        public List<CheckListItem> Defcon4SelectedItem { get { return _defcon4selectedItems; } set { Set(ref _defcon4selectedItems, value); } }
        public List<CheckListItem> Defcon5SelectedItem { get { return _defcon5selectedItems; } set { Set(ref _defcon5selectedItems, value); } }
        public int DefconStatus { get { return _defconStatus; } set { Set(ref _defconStatus, value); } }
        public VisualState DefconVisualState { get { return _defconVisualState; } set { Set(ref _defconVisualState, value); } }
        public double ScreenWidth { get { return _screenWidth; } set { Set(ref _screenWidth, value); } }
        public double FontSize { get { return _fontSize; } set { Set(ref _fontSize, value); } }
        public Visibility AddIconVisibility { get { return _addIconVisibility; } set { Set(ref _addIconVisibility, value); } }
        //public Visibility CancelIconVisibility { get { return _cancelIconVisibility; } set { Set(ref _cancelIconVisibility, value); } }
        public Visibility DeleteIconVisibility { get { return _deleteIconVisibility; } set { Set(ref _deleteIconVisibility, value); } }
        public ListViewSelectionMode DefconListViewSelectionMode { get { return _defconListViewSelectionMode; } set { Set(ref _defconListViewSelectionMode, value); } }
        public object ObjectItem { get { return _objectItem; } set { Set(ref _objectItem, value); } }
        public SolidColorBrush Defcon1CheckRectangleFill { get { return _defcon1CheckRectangleFill; } set { Set(ref _defcon1CheckRectangleFill, value); } }
        public SolidColorBrush Defcon2CheckRectangleFill { get { return _defcon2CheckRectangleFill; } set { Set(ref _defcon2CheckRectangleFill, value); } }
        public SolidColorBrush Defcon3CheckRectangleFill { get { return _defcon3CheckRectangleFill; } set { Set(ref _defcon3CheckRectangleFill, value); } }
        public SolidColorBrush Defcon4CheckRectangleFill { get { return _defcon4CheckRectangleFill; } set { Set(ref _defcon4CheckRectangleFill, value); } }
        public SolidColorBrush Defcon5CheckRectangleFill { get { return _defcon5CheckRectangleFill; } set { Set(ref _defcon5CheckRectangleFill, value); } }
        public int Defcon1UnCheckedItems { get { return _defcon1UnCheckedItems; } set { Set(ref _defcon1UnCheckedItems, value); } }
        public int Defcon2UnCheckedItems { get { return _defcon2UnCheckedItems; } set { Set(ref _defcon2UnCheckedItems, value); } }
        public int Defcon3UnCheckedItems { get { return _defcon3UnCheckedItems; } set { Set(ref _defcon3UnCheckedItems, value); } }
        public int Defcon4UnCheckedItems { get { return _defcon4UnCheckedItems; } set { Set(ref _defcon4UnCheckedItems, value); } }
        public int Defcon5UnCheckedItems { get { return _defcon5UnCheckedItems; } set { Set(ref _defcon5UnCheckedItems, value); } }
        #endregion

        #region Constructor
        public CheckListPageViewModel()
        {
            //_noUpdate = false;
            selectedItems = new List<CheckListItem>();
            selectionModeEnabled = false;
            _isSender = false;
        }
        #endregion

        #region Methods
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            DeleteIconVisibility = Visibility.Collapsed;
            //CancelIconVisibility = Visibility.Collapsed;
            await LoadDefconStatusFromRoamingSettings();
            ScreenWidth = ApplicationView.GetForCurrentView().VisibleBounds.Width;
            _scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            Defcon1CheckRectangleFill = new SolidColorBrush(Colors.Green);
            Defcon2CheckRectangleFill = new SolidColorBrush(Colors.Green);
            Defcon3CheckRectangleFill = new SolidColorBrush(Colors.Green);
            Defcon4CheckRectangleFill = new SolidColorBrush(Colors.Green);
            Defcon5CheckRectangleFill = new SolidColorBrush(Colors.Green);

            FontSize = LoadFontSizeFromLocalSettings();

            Defcon1CheckList = await CheckListService.LoadCheckList(1);
            Defcon2CheckList = await CheckListService.LoadCheckList(2);
            Defcon3CheckList = await CheckListService.LoadCheckList(3);
            Defcon4CheckList = await CheckListService.LoadCheckList(4);
            Defcon5CheckList = await CheckListService.LoadCheckList(5);
            SetTextBoxWidth(_textBoxWidth);
            SetFontSize(FontSize);

            Defcon1CheckList.CollectionChanged += Defcon1CheckList_CollectionChanged;
            Defcon2CheckList.CollectionChanged += Defcon2CheckList_CollectionChanged;
            Defcon3CheckList.CollectionChanged += Defcon3CheckList_CollectionChanged;
            Defcon4CheckList.CollectionChanged += Defcon4CheckList_CollectionChanged;
            Defcon5CheckList.CollectionChanged += Defcon5CheckList_CollectionChanged;

            Defcon1UnCheckedItems = UncheckedItemsService.Count(Defcon1CheckList, 1, _defconStatus);
            Defcon2UnCheckedItems = UncheckedItemsService.Count(Defcon2CheckList, 2, _defconStatus);
            Defcon3UnCheckedItems = UncheckedItemsService.Count(Defcon3CheckList, 3, _defconStatus);
            Defcon4UnCheckedItems = UncheckedItemsService.Count(Defcon4CheckList, 4, _defconStatus);
            Defcon5UnCheckedItems = UncheckedItemsService.Count(Defcon5CheckList, 5, _defconStatus);

            Defcon1CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon1CheckList, 1, UncheckedItemsService.Count(Defcon1CheckList, 1, _defconStatus), _defconStatus);
            Defcon2CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon2CheckList, 2, UncheckedItemsService.Count(Defcon2CheckList, 2, _defconStatus), _defconStatus);
            Defcon3CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon3CheckList, 3, UncheckedItemsService.Count(Defcon3CheckList, 3, _defconStatus), _defconStatus);
            Defcon4CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon4CheckList, 4, UncheckedItemsService.Count(Defcon4CheckList, 4, _defconStatus), _defconStatus);
            Defcon5CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon5CheckList, 5, UncheckedItemsService.Count(Defcon5CheckList, 5, _defconStatus), _defconStatus);
            UpdateTileBadge();
            _noUpdate = false;

            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("useTransparentTile")) _useTransparentTile = (bool)localSettings.Values["useTransparentTile"];
            if (localSettings.Values.ContainsKey("lanBroadcastIsOn") && (bool)localSettings.Values["lanBroadcastIsOn"])
            {
                _datagramService = new DatagramSocketService();
                await _datagramService.StartListener();
                _datagramService.IncomingMessageReceived += async (s, e) =>
                {
                    int.TryParse(e, out int defconStatus);
                    if (defconStatus > 0 && defconStatus < 6)
                    {
                        DefconStatus = defconStatus;
                        ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                        roamingSettings.Values["defconStatus"] = e;
                        LiveTileService.SetLiveTile(_defconStatus, _useTransparentTile);
                    }
                    if (defconStatus == 0 && !_isSender)
                    {
                        if (_streamSocketService != null) await _streamSocketService.ReceiveStringData(_datagramService.RemoteAddress);
                    }
                    _isSender = false;
                };
            }
            if (localSettings.Values.ContainsKey("lanMulticastIsOn") && (bool)localSettings.Values["lanMulticastIsOn"])
            {
                _streamSocketService = new StreamSocketService();
                await _streamSocketService.StartListener();
                _streamSocketService.IncomingChecklistReceived += async (s, e) => 
                {
                    Defcon1CheckList = await CheckListService.LoadCheckList(1);
                    Defcon2CheckList = await CheckListService.LoadCheckList(2);
                    Defcon3CheckList = await CheckListService.LoadCheckList(3);
                    Defcon4CheckList = await CheckListService.LoadCheckList(4);
                    Defcon5CheckList = await CheckListService.LoadCheckList(5);
                };
            }
        }
        public override async Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if (_datagramService != null) await _datagramService.Dispose();
            if (_streamSocketService != null) await _streamSocketService.Dispose();
        }
        private async void Defcon1CheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!_noUpdate)
            {
                if ((e == null && sender == null) || (sender as ItemObservableCollection<CheckListItem>)[e.NewStartingIndex].Item.Length > 0)
                {
                    //_noUpdate = true;
                    //Defcon1CheckList.Where(x => x.UnixTimeStampCreated == ((CheckListItem)e.NewItems[0]).UnixTimeStampCreated).FirstOrDefault().UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                    if (e != null)
                    {
                        for (int i = 0; i < Defcon1CheckList.Count; i++)
                        {
                            if (Defcon1CheckList[i].UnixTimeStampCreated == ((CheckListItem)e.NewItems[0]).UnixTimeStampCreated)
                            {
                                _noUpdate = true;
                                Defcon1CheckList[i].UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            }
                        }
                        _noUpdate = false;
                    }
                    await CheckListService.SaveCheckList(Defcon1CheckList, 1);
                    Defcon1UnCheckedItems = UncheckedItemsService.Count(Defcon1CheckList, 1, _defconStatus);
                    Defcon1CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon1CheckList, 1, UncheckedItemsService.Count(Defcon1CheckList, 1, _defconStatus), _defconStatus);
                    UpdateTileBadge();
                }
            }            
        }

        private async void Defcon2CheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!_noUpdate)
            {
                if ((e == null && sender == null) || (sender as ItemObservableCollection<CheckListItem>)[e.NewStartingIndex].Item.Length > 0)
                {
                    if (e != null)
                    {
                        for (int i = 0; i < Defcon2CheckList.Count; i++)
                        {
                            if (Defcon2CheckList[i].UnixTimeStampCreated == ((CheckListItem)e.NewItems[0]).UnixTimeStampCreated)
                            {
                                _noUpdate = true;
                                Defcon2CheckList[i].UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            }
                        }
                        _noUpdate = false;
                    }
                    await CheckListService.SaveCheckList(Defcon2CheckList, 2);
                    Defcon2UnCheckedItems = UncheckedItemsService.Count(Defcon2CheckList, 2, _defconStatus);
                    Defcon2CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon2CheckList, 2, UncheckedItemsService.Count(Defcon2CheckList, 2, _defconStatus), _defconStatus);
                    UpdateTileBadge();
                }
            }
        }

        private async void Defcon3CheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!_noUpdate)
            {
                if ((e == null && sender == null) || (sender as ItemObservableCollection<CheckListItem>)[e.NewStartingIndex].Item.Length > 0)
                {
                    if (e != null)
                    {
                        for (int i = 0; i < Defcon3CheckList.Count; i++)
                        {
                            if (Defcon3CheckList[i].UnixTimeStampCreated == ((CheckListItem)e.NewItems[0]).UnixTimeStampCreated)
                            {
                                _noUpdate = true;
                                Defcon3CheckList[i].UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            }
                        }
                        _noUpdate = false;
                    }
                    await CheckListService.SaveCheckList(Defcon3CheckList, 3);
                    Defcon3UnCheckedItems = UncheckedItemsService.Count(Defcon3CheckList, 3, _defconStatus);
                    Defcon3CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon3CheckList, 3, UncheckedItemsService.Count(Defcon3CheckList, 3, _defconStatus), _defconStatus);
                    UpdateTileBadge();
                }
            }
        }

        private async void Defcon4CheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!_noUpdate)
            {
                if ((e == null && sender == null) || (sender as ItemObservableCollection<CheckListItem>)[e.NewStartingIndex].Item.Length > 0)
                {
                    if (e != null)
                    {
                        for (int i = 0; i < Defcon4CheckList.Count; i++)
                        {
                            if (Defcon4CheckList[i].UnixTimeStampCreated == ((CheckListItem)e.NewItems[0]).UnixTimeStampCreated)
                            {
                                _noUpdate = true;
                                Defcon4CheckList[i].UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            }
                        }
                        _noUpdate = false;
                    }
                    await CheckListService.SaveCheckList(Defcon4CheckList, 4);
                    Defcon4UnCheckedItems = UncheckedItemsService.Count(Defcon4CheckList, 4, _defconStatus);
                    Defcon4CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon4CheckList, 4, UncheckedItemsService.Count(Defcon4CheckList, 4, _defconStatus), _defconStatus);
                    UpdateTileBadge();
                }
            }
        }

        private async void Defcon5CheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!_noUpdate)
            {
                if ((e == null && sender == null) || (sender as ItemObservableCollection<CheckListItem>)[e.NewStartingIndex].Item.Length > 0)
                {
                    if (e != null)
                    {
                        for (int i = 0; i < Defcon5CheckList.Count; i++)
                        {
                            if (Defcon5CheckList[i].UnixTimeStampCreated == ((CheckListItem)e.NewItems[0]).UnixTimeStampCreated)
                            {
                                _noUpdate = true;
                                Defcon5CheckList[i].UnixTimeStampUpdated = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                            }
                        }
                        _noUpdate = false;
                    }
                    await CheckListService.SaveCheckList(Defcon5CheckList, 5);
                    Defcon5UnCheckedItems = UncheckedItemsService.Count(Defcon5CheckList, 5, _defconStatus);
                    Defcon5CheckRectangleFill = UncheckedItemsService.CheckRectangleFill(Defcon5CheckList, 5, UncheckedItemsService.Count(Defcon5CheckList, 5, _defconStatus), _defconStatus);
                    UpdateTileBadge();
                }
            }
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            return Task.CompletedTask;
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

        private void SelectionMode(bool enabled)
        {
            if (enabled)
            {
                selectionModeEnabled = true;
                //CancelIconVisibility = Visibility.Collapsed;
                AddIconVisibility = Visibility.Collapsed;
                DeleteIconVisibility = Visibility.Visible;
                DefconListViewSelectionMode = ListViewSelectionMode.Multiple;
            }
            else
            {
                //CancelIconVisibility = Visibility.Collapsed;
                AddIconVisibility = Visibility.Visible;
                DeleteIconVisibility = Visibility.Collapsed;
                DefconListViewSelectionMode = ListViewSelectionMode.None;
                selectionModeEnabled = false;
            }
        }

        private void UpdateTileBadge()
        {
            if (LoadShowUncheckedItemsSetting())
            {
                int badgeNumber = UncheckedItemsService.CountBadgeNumber(_defconStatus, _defcon1CheckList, _defcon2CheckList, _defcon3CheckList, _defcon4CheckList, _defcon5CheckList);
                LiveTileService.UpdateTileBadge(badgeNumber);
                ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                roamingSettings.Values["badgeNumber"] = badgeNumber.ToString();
            }
        }

        private bool LoadShowUncheckedItemsSetting()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("showUncheckedItems")) return (bool)localSettings.Values["showUncheckedItems"];
            else return false;
        }

        private void SetFontSize(double value)
        {
            _noUpdate = true;
            foreach (var item in Defcon1CheckList)
            {
                item.FontSize = value;
            }
            foreach (var item in Defcon2CheckList)
            {
                item.FontSize = value;
            }
            foreach (var item in Defcon3CheckList)
            {
                item.FontSize = value;
            }
            foreach (var item in Defcon4CheckList)
            {
                item.FontSize = value;
            }
            foreach (var item in Defcon5CheckList)
            {
                item.FontSize = value;
            }
            _noUpdate = false;
        }

        private void SetTextBoxWidth(double value)
        {
            _noUpdate = true;
            foreach (var item in Defcon1CheckList)
            {
                item.Width = value;
            }
            foreach (var item in Defcon2CheckList)
            {
                item.Width = value;
            }
            foreach (var item in Defcon3CheckList)
            {
                item.Width = value;
            }
            foreach (var item in Defcon4CheckList)
            {
                item.Width = value;
            }
            foreach (var item in Defcon5CheckList)
            {
                item.Width = value;
            }
            _noUpdate = false;
        }

        private void SaveFontSizeToLocalSettings(double fontSize)
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values["fontSize"] = fontSize;
        }

        private double LoadFontSizeFromLocalSettings()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("fontSize")) return (double)localSettings.Values["fontSize"];
            else return 26;
        }
        #endregion

        #region Commands
        private DelegateCommand<string> _updateDefconVisualStateCommand;
        public DelegateCommand<string> UpdateDefconVisualStateCommand
        {
            get
            {
                if (_updateDefconVisualStateCommand != null)
                    return _updateDefconVisualStateCommand;
                _updateDefconVisualStateCommand = new DelegateCommand<string>
                    (
                        (s) =>
                        {
                            switch (int.Parse(s))
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
                            SelectionMode(false);
                        }
                    );
                return _updateDefconVisualStateCommand;
            }
        }

        private DelegateCommand _addItemCommand;
        public DelegateCommand AddItemCommand
        {
            get
            {
                if (_addItemCommand != null)
                    return _addItemCommand;
                _addItemCommand = new DelegateCommand
                    (
                        () =>
                        {
                            switch (DefconVisualState)
                            {
                                case VisualState.Defcon1VisualState:
                                    Defcon1CheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, FontSize = FontSize, Width = _textBoxWidth, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), DefconStatus = 1, Deleted = false, Visibility = Visibility.Visible });
                                    break;
                                case VisualState.Defcon2VisualState:
                                    Defcon2CheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, FontSize = FontSize, Width = _textBoxWidth, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), DefconStatus = 2, Deleted = false, Visibility = Visibility.Visible });
                                    break;
                                case VisualState.Defcon3VisualState:
                                    Defcon3CheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, FontSize = FontSize, Width = _textBoxWidth, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), DefconStatus = 3, Deleted = false, Visibility = Visibility.Visible });
                                    break;
                                case VisualState.Defcon4VisualState:
                                    Defcon4CheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, FontSize = FontSize, Width = _textBoxWidth, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), DefconStatus = 4, Deleted = false, Visibility = Visibility.Visible });
                                    break;
                                case VisualState.Defcon5VisualState:
                                    Defcon5CheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, FontSize = FontSize, Width = _textBoxWidth, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), DefconStatus = 5, Deleted = false, Visibility = Visibility.Visible });
                                    break;
                                default:
                                    break;
                            }
                            TextBox = string.Empty;
                        }
                    );
                return _addItemCommand;
            }
        }        

        private DelegateCommand<object> _shareCheckListCommand;
        public DelegateCommand<object> ShareCheckListCommand
        {
            get
            {
                if (_shareCheckListCommand != null)
                    return _shareCheckListCommand;
                _shareCheckListCommand = new DelegateCommand<object>
                    (
                        async (o) =>
                        {
                            (o as AppBarButton).Focus(FocusState.Programmatic);
                            await CheckListService.SaveCheckList(Defcon1CheckList, 1);
                            await CheckListService.SaveCheckList(Defcon2CheckList, 2);
                            await CheckListService.SaveCheckList(Defcon3CheckList, 3);
                            await CheckListService.SaveCheckList(Defcon4CheckList, 4);
                            await CheckListService.SaveCheckList(Defcon5CheckList, 5);
                            _isSender = true;
                            await _datagramService.SendMessage("0");
                        }
                    );
                return _shareCheckListCommand;
            }
        }

        private DelegateCommand<List<CheckListItem>> _selectionChangedCommand;
        public DelegateCommand<List<CheckListItem>> SelectionChangedCommand
        {
            get
            {
                if (_selectionChangedCommand != null)
                    return _selectionChangedCommand;
                _selectionChangedCommand = new DelegateCommand<List<CheckListItem>>
                    (
                        (o) =>
                        {
                            foreach (var item in o)
                            {
                                if (!selectedItems.Contains(item as CheckListItem))
                                {
                                    selectedItems.Add(item as CheckListItem);
                                }
                            }
                        }
                    );
                return _selectionChangedCommand;
            }
        }

        private DelegateCommand _enableSelectionModeCommand;
        public DelegateCommand EnableSelectionModeCommand
        {
            get
            {
                if (_enableSelectionModeCommand != null)
                    return _enableSelectionModeCommand;
                _enableSelectionModeCommand = new DelegateCommand
                    (
                        () =>
                        {
                            if (!selectionModeEnabled) SelectionMode(true);
                            else SelectionMode(false);
                        }
                    );
                return _enableSelectionModeCommand;
            }
        }

        private DelegateCommand _deleteSelectionCommand;

        public DelegateCommand DeleteSelectionCommand
        {
            get
            {
                if (_deleteSelectionCommand != null)
                    return _deleteSelectionCommand;
                _deleteSelectionCommand = new DelegateCommand
                    (
                        () =>
                        {
                            _noUpdate = true;
                            AddIconVisibility = Visibility.Visible;
                            DeleteIconVisibility = Visibility.Collapsed;
                            switch (DefconVisualState)
                            {
                                case VisualState.Defcon1VisualState:
                                    foreach (var item in selectedItems)
                                    {
                                        Defcon1CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Deleted = true;
                                        Defcon1CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Visibility = Visibility.Collapsed;
                                        //Defcon1CheckList.Remove(item);
                                    }
                                    _noUpdate = false;
                                    Defcon1CheckList_CollectionChanged(null, null);
                                    break;
                                case VisualState.Defcon2VisualState:
                                    foreach (var item in selectedItems)
                                    {
                                        Defcon2CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Deleted = true;
                                        Defcon2CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Visibility = Visibility.Collapsed;
                                        //Defcon2CheckList.Remove(item);
                                    }
                                    _noUpdate = false;
                                    Defcon2CheckList_CollectionChanged(null, null);
                                    break;
                                case VisualState.Defcon3VisualState:
                                    foreach (var item in selectedItems)
                                    {
                                        Defcon3CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Deleted = true;
                                        Defcon3CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Visibility = Visibility.Collapsed;
                                        //Defcon3CheckList.Remove(item);
                                    }
                                    _noUpdate = false;
                                    Defcon3CheckList_CollectionChanged(null, null);
                                    break;
                                case VisualState.Defcon4VisualState:
                                    foreach (var item in selectedItems)
                                    {
                                        Defcon4CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Deleted = true;
                                        Defcon4CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Visibility = Visibility.Collapsed;
                                        //Defcon4CheckList.Remove(item);
                                    }
                                    _noUpdate = false;
                                    Defcon4CheckList_CollectionChanged(null, null);
                                    break;
                                case VisualState.Defcon5VisualState:
                                    foreach (var item in selectedItems)
                                    {
                                        Defcon5CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Deleted = true;
                                        Defcon5CheckList.Where((c) => c.UnixTimeStampCreated == item.UnixTimeStampCreated).FirstOrDefault().Visibility = Visibility.Collapsed;
                                        //Defcon5CheckList.Remove(item);
                                    }
                                    _noUpdate = false;
                                    Defcon5CheckList_CollectionChanged(null, null);
                                    break;
                                default:
                                    break;
                            }
                            DefconListViewSelectionMode = ListViewSelectionMode.None;
                            selectedItems.Clear();
                        }
                    );
                return _deleteSelectionCommand;
            }
        }

        private DelegateCommand _fontDecreaseCommand;
        public DelegateCommand FontDecreaseCommand
        {
            get
            {
                if (_fontDecreaseCommand != null)
                    return _fontDecreaseCommand;
                _fontDecreaseCommand = new DelegateCommand
                    (
                        () =>
                        {
                            FontSize--;
                            SetFontSize(FontSize);
                            SaveFontSizeToLocalSettings(FontSize);
                        }
                    );
                return _fontDecreaseCommand;
            }
        }

        private DelegateCommand _fontIncreaseCommand;
        public DelegateCommand FontIncreaseCommand
        {
            get
            {
                if (_fontIncreaseCommand != null)
                    return _fontIncreaseCommand;
                _fontIncreaseCommand = new DelegateCommand
                    (
                        () =>
                        {
                            FontSize++;
                            SetFontSize(FontSize);
                            SaveFontSizeToLocalSettings(FontSize);
                        }
                    );
                return _fontIncreaseCommand;
            }
        }

        private DelegateCommand<SizeChangedEventArgs> _windowSizeChangedCommand;
        public DelegateCommand<SizeChangedEventArgs> WindowSizeChangedCommand
        {
            get
            {
                if (_windowSizeChangedCommand != null)
                    return _windowSizeChangedCommand;
                _windowSizeChangedCommand = new DelegateCommand<SizeChangedEventArgs>
                    (
                        async (args) =>
                        {
                            if (Defcon1CheckList == null || Defcon2CheckList == null || Defcon3CheckList == null || Defcon4CheckList == null || Defcon5CheckList == null)
                            {
                                Defcon1CheckList = await CheckListService.LoadCheckList(1);
                                Defcon2CheckList = await CheckListService.LoadCheckList(2);
                                Defcon3CheckList = await CheckListService.LoadCheckList(3);
                                Defcon4CheckList = await CheckListService.LoadCheckList(4);
                                Defcon5CheckList = await CheckListService.LoadCheckList(5);
                            }
                            SetTextBoxWidth(args.NewSize.Width - 52);
                            _textBoxWidth = (args.NewSize.Width - 52);
                            _noUpdate = false;
                        }
                    );
                return _windowSizeChangedCommand;
            }
        }
        #endregion
    }
}