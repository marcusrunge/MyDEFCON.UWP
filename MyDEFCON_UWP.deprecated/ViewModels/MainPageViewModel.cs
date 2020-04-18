using Models;
using MyDEFCON_UWP.Services;
using Services;
using SocketLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Template10.Mvvm;
using Template10.Services.NavigationService;
using Windows.ApplicationModel.DataTransfer;
using Windows.Networking;
using Windows.Networking.Connectivity;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace MyDEFCON_UWP.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        #region Fields
        int _defconStatus;
        string _shareMessage;
        bool _Defcon1ButtonIsChecked;
        bool _Defcon2ButtonIsChecked;
        bool _Defcon3ButtonIsChecked;
        bool _Defcon4ButtonIsChecked;
        bool _Defcon5ButtonIsChecked;
        bool _useTransparentTile = default(bool);
        double _screenWidth;
        Visibility _shareIconVisibility;
        Visibility _cancelIconVisibility;
        string _textBox;
        ItemObservableCollection<CheckListItem> _defcon1CheckList;
        ItemObservableCollection<CheckListItem> _defcon2CheckList;
        ItemObservableCollection<CheckListItem> _defcon3CheckList;
        ItemObservableCollection<CheckListItem> _defcon4CheckList;
        ItemObservableCollection<CheckListItem> _defcon5CheckList;
        bool loadFromRoaming = false;
        DatagramSocketService _datagramService;
        bool lanBroadcastIsOn = false;
        bool _isSetDefconCommandBlocked = false;
        double _fontSize;
        #endregion

        #region Properties
        public bool Defcon1ButtonIsChecked { get { return _Defcon1ButtonIsChecked; } set { Set(ref _Defcon1ButtonIsChecked, value); } }
        public bool Defcon2ButtonIsChecked { get { return _Defcon2ButtonIsChecked; } set { Set(ref _Defcon2ButtonIsChecked, value); } }
        public bool Defcon3ButtonIsChecked { get { return _Defcon3ButtonIsChecked; } set { Set(ref _Defcon3ButtonIsChecked, value); } }
        public bool Defcon4ButtonIsChecked { get { return _Defcon4ButtonIsChecked; } set { Set(ref _Defcon4ButtonIsChecked, value); } }
        public bool Defcon5ButtonIsChecked { get { return _Defcon5ButtonIsChecked; } set { Set(ref _Defcon5ButtonIsChecked, value); } }
        public double ScreenWidth { get { return _screenWidth; } set { Set(ref _screenWidth, value); } }
        public Visibility ShareIconVisibility { get { return _shareIconVisibility; } set { Set(ref _shareIconVisibility, value); } }
        public Visibility CancelIconVisibility { get { return _cancelIconVisibility; } set { Set(ref _cancelIconVisibility, value); } }
        public string TextBox { get { return _textBox; } set { Set(ref _textBox, value); } }
        public double FontSize { get { return _fontSize; } set { Set(ref _fontSize, value); } }
        #endregion

        #region Constructor
        public MainPageViewModel()
        {
            Window.Current.SizeChanged += Current_SizeChanged;
        }
        #endregion

        #region Methods
        public void toggleButton_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            double calculatedTextBlockWidth = e.NewSize.Height * 0.5 * 6.5;
            if (e.NewSize.Width > calculatedTextBlockWidth)
            {
                FontSize = Math.Floor(e.NewSize.Height * 0.5);
            }
            else
            {
                FontSize = Math.Floor(e.NewSize.Width / 6.5);
            }
        }

        private void Current_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            ScreenWidth = ApplicationView.GetForCurrentView().VisibleBounds.Width;
        }
        public async override Task OnNavigatedToAsync(object parameter, NavigationMode mode, IDictionary<string, object> state)
        {
            UncheckOtherButton(0);
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            LoadDefconStatusFromRoamingSettings();
            SetDefconStatus(_defconStatus);
            LoadTransparentTileSetting();
            LiveTileManagement.SetLiveTile(_defconStatus, _useTransparentTile);
            _defcon1CheckList = await CheckListManagement.LoadCheckList(1);
            _defcon2CheckList = await CheckListManagement.LoadCheckList(2);
            _defcon3CheckList = await CheckListManagement.LoadCheckList(3);
            _defcon4CheckList = await CheckListManagement.LoadCheckList(4);
            _defcon5CheckList = await CheckListManagement.LoadCheckList(5);
            DataTransferManager.GetForCurrentView().DataRequested += OnShareDataRequested;
            ScreenWidth = ApplicationView.GetForCurrentView().VisibleBounds.Width;
            CancelIconVisibility = Visibility.Collapsed;
            ApplicationData.Current.DataChanged += (s, e) =>
            {
                _defconStatus = Convert.ToInt16(s.RoamingSettings.Values["defconStatus"]);
                SetDefconStatus(_defconStatus);
                LiveTileManagement.SetLiveTile(_defconStatus, _useTransparentTile);
                ReverseUncheck(_defconStatus);
                UpdateTileBadge();
            };

            if (localSettings.Values.ContainsKey("lanBroadcastIsOn") && (bool)localSettings.Values["lanBroadcastIsOn"])
            {
                lanBroadcastIsOn = true;
                _datagramService = new DatagramSocketService();
                await _datagramService.StartListener();
                _datagramService.IncomingMessageReceived += (s, e) =>
                {
                    if (!(GetLocalIp().Equals((s as DatagramSocketService).RemoteAddress.CanonicalName)) && int.TryParse(e, out int defconStatus) && (defconStatus > 0 && defconStatus < 6))
                    {
                        _defconStatus = defconStatus;
                        ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
                        roamingSettings.Values["defconStatus"] = e;
                        _isSetDefconCommandBlocked = true;
                        UncheckOtherButton(0);
                        SetDefconStatus(_defconStatus);
                        _isSetDefconCommandBlocked = false;
                        ReverseUncheck(_defconStatus);
                        LiveTileManagement.SetLiveTile(_defconStatus, _useTransparentTile);
                    }
                };
            }
        }

        private void OnShareDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            var dataPackage = args.Request.Data;
            var deferral = args.Request.GetDeferral();
            //var imageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(string.Format("ms-appx:///ShareImages/Defcon{0}.png", _defconStatus.ToString())));
            dataPackage.Properties.Title = "DEFCON STATUS";
            dataPackage.Properties.Description = "DEFCON Status Payload for sharing";
            if (_shareMessage != null && _shareMessage.Length > 0) { dataPackage.SetText(_shareMessage); }
            else { dataPackage.SetText("updated to"); }
            //dataPackage.SetStorageItems(new List<StorageFile> { imageFile });
            dataPackage.SetBitmap(RandomAccessStreamReference.CreateFromUri(new Uri(string.Format("ms-appx:///ShareImages/Defcon{0}.png", _defconStatus.ToString()))));
            deferral.Complete();
        }

        public override async Task OnNavigatedFromAsync(IDictionary<string, object> state, bool suspending)
        {
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
            if (_datagramService != null) await _datagramService.Dispose();
            //UncheckOtherButton(0);
            //return Task.CompletedTask;
        }

        public override Task OnNavigatingFromAsync(NavigatingEventArgs args)
        {
            args.Cancel = false;
            DataTransferManager.GetForCurrentView().DataRequested -= OnShareDataRequested;
            return Task.CompletedTask;
        }

        private void LoadDefconStatusFromRoamingSettings()
        {
            loadFromRoaming = true;
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            if (roamingSettings.Values.ContainsKey("defconStatus"))
            {
                //await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                _defconStatus = Convert.ToInt16(roamingSettings.Values["defconStatus"].ToString());
                SetDefconStatus(_defconStatus);
            }
            else
            {
                _defconStatus = 5;
                Defcon5ButtonIsChecked = true;
            }
            loadFromRoaming = false;
            //return Task.FromResult<object>(null);
        }

        private void LoadTransparentTileSetting()
        {
            ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
            if (localSettings.Values.ContainsKey("useTransparentTile")) _useTransparentTile = (bool)localSettings.Values["useTransparentTile"];
            else _useTransparentTile = false;
        }

        private void SetDefconStatus(int status)
        {
            switch (status)
            {
                case 1:
                    Defcon1ButtonIsChecked = true;
                    break;
                case 2:
                    Defcon2ButtonIsChecked = true;
                    break;
                case 3:
                    Defcon3ButtonIsChecked = true;
                    break;
                case 4:
                    Defcon4ButtonIsChecked = true;
                    break;
                case 5:
                    Defcon5ButtonIsChecked = true;
                    break;
                default:
                    Defcon5ButtonIsChecked = true;
                    break;
            }
        }

        private void SaveDefconStatus(int status)
        {
            ApplicationDataContainer roamingSettings = ApplicationData.Current.RoamingSettings;
            roamingSettings.Values["defconStatus"] = status.ToString();
        }

        private void UncheckOtherButton(int v)
        {
            switch (v)
            {
                case 0:
                    Defcon1ButtonIsChecked = false;
                    Defcon2ButtonIsChecked = false;
                    Defcon3ButtonIsChecked = false;
                    Defcon4ButtonIsChecked = false;
                    Defcon5ButtonIsChecked = false;
                    break;
                case 1:
                    Defcon1ButtonIsChecked = false;
                    break;
                case 2:
                    Defcon2ButtonIsChecked = false;
                    break;
                case 3:
                    Defcon3ButtonIsChecked = false;
                    break;
                case 4:
                    Defcon4ButtonIsChecked = false;
                    break;
                case 5:
                    Defcon5ButtonIsChecked = false;
                    break;
                default:
                    break;
            }
        }

        private void UpdateTileBadge()
        {
            if (LoadShowUncheckedItemsSetting())
            {
                int badgeNumber = UncheckedItemsService.CountBadgeNumber(_defconStatus, _defcon1CheckList, _defcon2CheckList, _defcon3CheckList, _defcon4CheckList, _defcon5CheckList);
                LiveTileManagement.UpdateTileBadge(badgeNumber);
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

        private async void ReverseUncheck(int defconStatus)
        {
            switch (defconStatus)
            {
                case 2:
                    if (_defconStatus != 2)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        await CheckListManagement.SaveCheckList(_defcon1CheckList, 1);
                    }

                    break;
                case 3:
                    if (_defconStatus != 3)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        _defcon2CheckList = UncheckCollection(_defcon2CheckList);
                        await CheckListManagement.SaveCheckList(_defcon1CheckList, 1);
                        await CheckListManagement.SaveCheckList(_defcon2CheckList, 2);
                    }

                    break;
                case 4:
                    if (_defconStatus != 4)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        _defcon2CheckList = UncheckCollection(_defcon2CheckList);
                        _defcon3CheckList = UncheckCollection(_defcon3CheckList);
                        await CheckListManagement.SaveCheckList(_defcon1CheckList, 1);
                        await CheckListManagement.SaveCheckList(_defcon2CheckList, 2);
                        await CheckListManagement.SaveCheckList(_defcon3CheckList, 3);
                    }

                    break;
                case 5:
                    if (_defconStatus != 5)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        _defcon2CheckList = UncheckCollection(_defcon2CheckList);
                        _defcon3CheckList = UncheckCollection(_defcon3CheckList);
                        _defcon4CheckList = UncheckCollection(_defcon4CheckList);
                        await CheckListManagement.SaveCheckList(_defcon1CheckList, 1);
                        await CheckListManagement.SaveCheckList(_defcon2CheckList, 2);
                        await CheckListManagement.SaveCheckList(_defcon3CheckList, 3);
                        await CheckListManagement.SaveCheckList(_defcon4CheckList, 4);
                    }
                    break;
                default:
                    break;
            }
        }

        private ItemObservableCollection<CheckListItem> UncheckCollection(ItemObservableCollection<CheckListItem> collection)
        {
            foreach (var item in collection)
            {
                item.Checked = false;
            }
            return collection;
        }
        #endregion

        #region Commands
        private DelegateCommand _setDefcon1Command;

        public DelegateCommand SetDefcon1Command
        {
            get
            {
                if (_setDefcon1Command == null)
                {
                    _setDefcon1Command = new DelegateCommand(() =>
                    {
                        if (!_isSetDefconCommandBlocked)
                        {
                            UncheckOtherButton(_defconStatus);
                            if (!Defcon2ButtonIsChecked && !Defcon3ButtonIsChecked && !Defcon4ButtonIsChecked && !Defcon5ButtonIsChecked)
                            {
                                Defcon1ButtonIsChecked = true;
                                _defconStatus = 1;
                                LiveTileManagement.SetLiveTile(1, _useTransparentTile);
                                //if (!loadFromRoaming) ReverseUncheck(1);
                                UpdateTileBadge();
                                SaveDefconStatus(1);
                                if (lanBroadcastIsOn) _datagramService?.SendMessage("1");
                            }
                        }
                    });
                }
                return _setDefcon1Command;
            }
        }

        private DelegateCommand _setDefcon2Command;

        public DelegateCommand SetDefcon2Command
        {
            get
            {
                if (_setDefcon2Command == null)
                {
                    _setDefcon2Command = new DelegateCommand(() =>
                    {
                        if (!_isSetDefconCommandBlocked)
                        {
                            UncheckOtherButton(_defconStatus);
                            if (!Defcon1ButtonIsChecked && !Defcon3ButtonIsChecked && !Defcon4ButtonIsChecked && !Defcon5ButtonIsChecked)
                            {
                                Defcon2ButtonIsChecked = true;
                                //_defconStatus = 2;
                                LiveTileManagement.SetLiveTile(2, _useTransparentTile);
                                if (!loadFromRoaming) ReverseUncheck(2);
                                _defconStatus = 2;
                                UpdateTileBadge();
                                SaveDefconStatus(2);
                                if (lanBroadcastIsOn) _datagramService?.SendMessage("2");
                            }
                        }
                    });
                }
                return _setDefcon2Command;
            }
        }

        private DelegateCommand _setDefcon3Command;

        public DelegateCommand SetDefcon3Command
        {
            get
            {
                if (_setDefcon3Command == null)
                {
                    _setDefcon3Command = new DelegateCommand(() =>
                    {
                        if (!_isSetDefconCommandBlocked)
                        {
                            UncheckOtherButton(_defconStatus);
                            if (!Defcon1ButtonIsChecked && !Defcon2ButtonIsChecked && !Defcon4ButtonIsChecked && !Defcon5ButtonIsChecked)
                            {
                                Defcon3ButtonIsChecked = true;
                                //_defconStatus = 3;
                                LiveTileManagement.SetLiveTile(3, _useTransparentTile);
                                if (!loadFromRoaming) ReverseUncheck(3);
                                _defconStatus = 3;
                                UpdateTileBadge();
                                SaveDefconStatus(3);
                                if (lanBroadcastIsOn) _datagramService?.SendMessage("3");
                            }
                        }
                    });
                }
                return _setDefcon3Command;
            }
        }

        private DelegateCommand _setDefcon4Command;

        public DelegateCommand SetDefcon4Command
        {
            get
            {
                if (_setDefcon4Command == null)
                {
                    _setDefcon4Command = new DelegateCommand(() =>
                    {
                        if (!_isSetDefconCommandBlocked)
                        {
                            UncheckOtherButton(_defconStatus);
                            if (!Defcon1ButtonIsChecked && !Defcon2ButtonIsChecked && !Defcon3ButtonIsChecked && !Defcon5ButtonIsChecked)
                            {
                                Defcon4ButtonIsChecked = true;
                                //_defconStatus = 4;
                                LiveTileManagement.SetLiveTile(4, _useTransparentTile);
                                if (!loadFromRoaming) ReverseUncheck(4);
                                _defconStatus = 4;
                                UpdateTileBadge();
                                SaveDefconStatus(4);
                                if (lanBroadcastIsOn) _datagramService?.SendMessage("4");
                            }
                        }
                    });
                }
                return _setDefcon4Command;
            }
        }

        private DelegateCommand _setDefcon5Command;

        public DelegateCommand SetDefcon5Command
        {
            get
            {
                if (_setDefcon5Command == null)
                {
                    _setDefcon5Command = new DelegateCommand(() =>
                    {
                        if (!_isSetDefconCommandBlocked)
                        {
                            UncheckOtherButton(_defconStatus);
                            if (!Defcon1ButtonIsChecked && !Defcon2ButtonIsChecked && !Defcon3ButtonIsChecked && !Defcon4ButtonIsChecked)
                            {
                                Defcon5ButtonIsChecked = true;
                                //_defconStatus = 5;
                                LiveTileManagement.SetLiveTile(5, _useTransparentTile);
                                if (!loadFromRoaming) ReverseUncheck(5);
                                _defconStatus = 5;
                                UpdateTileBadge();
                                SaveDefconStatus(5);
                                if (lanBroadcastIsOn) _datagramService?.SendMessage("5");
                            }
                        }
                    });
                }
                return _setDefcon5Command;
            }
        }

        private DelegateCommand<string> _shareCommand;
        public DelegateCommand<string> ShareCommand
        {
            get
            {
                if (_shareCommand != null)
                    return _shareCommand;
                _shareCommand = new DelegateCommand<string>
                    (
                        (s) =>
                        {
                            _shareMessage = s;
                            DataTransferManager.ShowShareUI();
                            TextBox = string.Empty;
                        }
                    );
                return _shareCommand;
            }
        }

        private DelegateCommand _shareFlyoutOpenCommand;
        public DelegateCommand ShareFlyoutOpenCommand
        {
            get
            {
                if (_shareFlyoutOpenCommand != null)
                    return _shareFlyoutOpenCommand;
                _shareFlyoutOpenCommand = new DelegateCommand
                    (
                        () =>
                        {
                            ShareIconVisibility = Visibility.Collapsed;
                            CancelIconVisibility = Visibility.Visible;
                        }
                    );
                return _shareFlyoutOpenCommand;
            }
        }

        private DelegateCommand _shareFlyoutCloseCommand;
        public DelegateCommand ShareFlyoutCloseCommand
        {
            get
            {
                if (_shareFlyoutCloseCommand != null)
                    return _shareFlyoutCloseCommand;
                _shareFlyoutCloseCommand = new DelegateCommand
                    (
                        () =>
                        {
                            CancelIconVisibility = Visibility.Collapsed;
                            ShareIconVisibility = Visibility.Visible;
                            TextBox = string.Empty;
                        }
                    );
                return _shareFlyoutCloseCommand;
            }
        }
        #endregion
        public string GetLocalIp(HostNameType hostNameType = HostNameType.Ipv4)
        {
            var internetConnectionProfile = NetworkInformation.GetInternetConnectionProfile();
            if (internetConnectionProfile?.NetworkAdapter == null) return null;
            var hostname = NetworkInformation.GetHostNames().FirstOrDefault(h => h.Type == hostNameType && h.IPInformation?.NetworkAdapter != null && h.IPInformation.NetworkAdapter.NetworkAdapterId == internetConnectionProfile.NetworkAdapter.NetworkAdapterId);
            return hostname?.CanonicalName;
        }
    }
}