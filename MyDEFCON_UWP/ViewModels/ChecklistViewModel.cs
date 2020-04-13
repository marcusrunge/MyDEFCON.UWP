
using Models;
using MyDEFCON_UWP.Helpers;
using Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace MyDEFCON_UWP.ViewModels
{
    public class ChecklistViewModel : Observable
    {
        private IEventService _eventService;
        private long[] selectedItemsUnixTimeStampCreated;

        private int _defconStatus;
        public int DefconStatus { get => _defconStatus; set => Set(ref _defconStatus, value); }

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

        public ChecklistViewModel(IEventService eventService)
        {
            _eventService = eventService;
            DefconStatus = int.Parse(StorageService.GetSetting("defconStatus", "5", StorageService.StorageStrategies.Roaming));
            SelectedItemsUnixTimeStampCreated = new List<long>();            
        }

        private ICommand _loadedCommand;
        public ICommand LoadedCommand => _loadedCommand ?? (_loadedCommand = new RelayCommand<object>(async (param) =>
        {
            DefconCheckList = await CheckListService.LoadCheckList(DefconStatus);
            DefconCheckList.CollectionChanged += DefconCheckList_CollectionChanged;
            _eventService.AppBarButtonClicked += AppBarButtonClicked;
            var defcon1CheckList = await CheckListService.LoadCheckList(1);
            var defcon2CheckList = await CheckListService.LoadCheckList(2);
            var defcon3CheckList = await CheckListService.LoadCheckList(3);
            var defcon4CheckList = await CheckListService.LoadCheckList(4);
            var defcon5CheckList = await CheckListService.LoadCheckList(5);
            Defcon1UnCheckedItems = UncheckedItems.Count(defcon1CheckList, 1, DefconStatus);
            Defcon2UnCheckedItems = UncheckedItems.Count(defcon2CheckList, 2, DefconStatus);
            Defcon3UnCheckedItems = UncheckedItems.Count(defcon3CheckList, 3, DefconStatus);
            Defcon4UnCheckedItems = UncheckedItems.Count(defcon4CheckList, 4, DefconStatus);
            Defcon5UnCheckedItems = UncheckedItems.Count(defcon5CheckList, 5, DefconStatus);
            Defcon1RectangleFill = UncheckedItems.RectangleFill(defcon1CheckList, 1, Defcon1UnCheckedItems, DefconStatus);
            Defcon2RectangleFill = UncheckedItems.RectangleFill(defcon2CheckList, 2, Defcon2UnCheckedItems, DefconStatus);
            Defcon3RectangleFill = UncheckedItems.RectangleFill(defcon3CheckList, 3, Defcon3UnCheckedItems, DefconStatus);
            Defcon4RectangleFill = UncheckedItems.RectangleFill(defcon4CheckList, 4, Defcon4UnCheckedItems, DefconStatus);
            Defcon5RectangleFill = UncheckedItems.RectangleFill(defcon5CheckList, 5, Defcon5UnCheckedItems, DefconStatus);
        }));

        private async void DefconCheckList_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (DefconStatus)
            {
                case 1:
                    Defcon1UnCheckedItems = UncheckedItems.Count(DefconCheckList, 1, DefconStatus);
                    Defcon1RectangleFill = UncheckedItems.RectangleFill(DefconCheckList, 1, Defcon1UnCheckedItems, DefconStatus);
                    break;
                case 2:
                    Defcon2UnCheckedItems = UncheckedItems.Count(DefconCheckList, 2, DefconStatus);
                    Defcon2RectangleFill = UncheckedItems.RectangleFill(DefconCheckList, 2, Defcon2UnCheckedItems, DefconStatus);
                    break;
                case 3:
                    Defcon3UnCheckedItems = UncheckedItems.Count(DefconCheckList, 3, DefconStatus);
                    Defcon3RectangleFill = UncheckedItems.RectangleFill(DefconCheckList, 3, Defcon3UnCheckedItems, DefconStatus);
                    break;
                case 4:
                    Defcon4UnCheckedItems = UncheckedItems.Count(DefconCheckList, 4, DefconStatus);
                    Defcon4RectangleFill = UncheckedItems.RectangleFill(DefconCheckList, 4, Defcon4UnCheckedItems, DefconStatus);
                    break;
                case 5:
                    Defcon5UnCheckedItems = UncheckedItems.Count(DefconCheckList, 5, DefconStatus);
                    Defcon5RectangleFill = UncheckedItems.RectangleFill(DefconCheckList, 5, Defcon5UnCheckedItems, DefconStatus);
                    break;
                default:
                    break;
            } if(selectedItemsUnixTimeStampCreated !=null&&selectedItemsUnixTimeStampCreated.Length==0)
            await CheckListService.SaveCheckList(DefconCheckList, DefconStatus);
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
                default:
                    break;
            }
        }

        private async Task DeleteSelectedItems()
        {
            selectedItemsUnixTimeStampCreated = new long[SelectedItemsUnixTimeStampCreated.Count];
            SelectedItemsUnixTimeStampCreated.CopyTo(selectedItemsUnixTimeStampCreated);
            for (int i = 0; i < selectedItemsUnixTimeStampCreated.Length; i++)
            {
                for (int j = 0; j < DefconCheckList.Count; j++)
                {
                    if (DefconCheckList[j].UnixTimeStampCreated == selectedItemsUnixTimeStampCreated[i])
                    {
                        DefconCheckList[j].Deleted = true;
                        DefconCheckList[j].Visibility = Visibility.Collapsed;
                        DefconCheckList[j].Checked = true;
                    }
                }
            }
            await CheckListService.SaveCheckList(DefconCheckList, DefconStatus);
        }

        private void AddItemToChecklist()
        {
            DefconCheckList.Add(new CheckListItem() { Item = string.Empty, Checked = false, DefconStatus = (short)DefconStatus, FontSize = 14, UnixTimeStampCreated = DateTimeOffset.Now.ToUnixTimeMilliseconds(), Deleted = false, Visibility = Visibility.Visible });
        }

        private ICommand _loadDefconChecklistCommand;
        public ICommand LoadDefconChecklistCommand => _loadDefconChecklistCommand ?? (_loadDefconChecklistCommand = new RelayCommand<object>(async (param) =>
        {
            CheckistSelectionMode = ListViewSelectionMode.None;
            _eventService.OnChecklistChanged(null);
            DefconCheckList.CollectionChanged -= DefconCheckList_CollectionChanged;
            DefconStatus = int.Parse(param as string);
            DefconCheckList?.Clear();
            try
            {
                DefconCheckList = await CheckListService.LoadCheckList(DefconStatus);
            }
            catch { }
            DefconCheckList.CollectionChanged += DefconCheckList_CollectionChanged;

        }));

        private ICommand _unloadedCommand;
        public ICommand UnloadedCommand => _unloadedCommand ?? (_unloadedCommand = new RelayCommand<object>(async (param) =>
        {
            await CheckListService.SaveCheckList(DefconCheckList, DefconStatus);
        }));
    }
}
