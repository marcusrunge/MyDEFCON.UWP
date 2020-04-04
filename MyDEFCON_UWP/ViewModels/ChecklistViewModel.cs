
using Models;
using MyDEFCON_UWP.Helpers;
using Services;
using System;
using System.Windows.Input;
using Windows.UI.Xaml;

namespace MyDEFCON_UWP.ViewModels
{
    public class ChecklistViewModel : Observable
    {
        private IEventService _eventService;
        double _textBoxWidth;

        private int _defconStatus;
        public int DefconStatus { get => _defconStatus; set => Set(ref _defconStatus, value); }

        private double _fontSize;
        public double FontSize { get => _fontSize; set => Set(ref _fontSize, value); }

        ItemObservableCollection<CheckListItem> _defconCheckList;
        public ItemObservableCollection<CheckListItem> DefconCheckList { get { return _defconCheckList; } set { Set(ref _defconCheckList, value); } }

        public ChecklistViewModel(IEventService eventService)
        {
            _eventService = eventService;
            DefconStatus = int.Parse(StorageService.GetSetting("defconStatus", "5", StorageService.StorageStrategies.Roaming));
            FontSize = 14;
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>(async (param) =>
        {            
            DefconCheckList= await CheckListService.LoadCheckList(DefconStatus);
            _eventService.AppBarButtonClicked += AppBarButtonClicked;
        }));

        private void AppBarButtonClicked(object sender, EventArgs e)
        {
            switch ((e as AppBarButtonClickedEventArgs).Button)
            {
                case "Add":
                    AddItemToChecklist();                    
                    break;
                default:
                    break;
            }
        }

        private void AddItemToChecklist()
        {
            DefconCheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, DefconStatus = (short)DefconStatus, FontSize=FontSize, UnixTimeStampCreated= DateTimeOffset.Now.ToUnixTimeMilliseconds(), Deleted = false, Visibility = Visibility.Visible , Width=_textBoxWidth});
        }

        private ICommand _setFontSizeCommand;
        public ICommand SetFontSizeCommand => _setFontSizeCommand ?? (_setFontSizeCommand = new RelayCommand<object>((param) =>
        {
            FontSize = Math.Floor((param as SizeChangedEventArgs).NewSize.Width / 6.5);
        }));

        private ICommand _loadDefconChecklistCommand;
        public ICommand LoadDefconChecklistCommand => _loadDefconChecklistCommand ?? (_loadDefconChecklistCommand = new RelayCommand<object>(async (param) =>
        {
            DefconStatus = int.Parse(param as string);
            DefconCheckList?.Clear();            
            DefconCheckList = await CheckListService.LoadCheckList(DefconStatus);
        }));

        private ICommand _unloadedCommand;
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>((param) =>
        {            
        }));

        private ICommand _windowSizeChangedCommand;
        public ICommand WindowSizeChangedCommand => _windowSizeChangedCommand ?? (_windowSizeChangedCommand = new RelayCommand<SizeChangedEventArgs>((param) =>
        {
            _textBoxWidth = (param.NewSize.Width - 52);
        }));
    }
}
