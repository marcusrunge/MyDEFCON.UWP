using Models;
using Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Checklists
{
    internal abstract class ChecklistsBase : INotifyPropertyChanged
    {
        private protected static ItemObservableCollection<CheckListItem> _defcon1CheckList;
        private protected static ItemObservableCollection<CheckListItem> _defcon2CheckList;
        private protected static ItemObservableCollection<CheckListItem> _defcon3CheckList;
        private protected static ItemObservableCollection<CheckListItem> _defcon4CheckList;
        private protected static ItemObservableCollection<CheckListItem> _defcon5CheckList;

        internal protected async Task InitializeBase()
        {
            _defcon1CheckList = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon1.json", StorageManagement.StorageStrategies.Roaming);
            _defcon2CheckList = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon2.json", StorageManagement.StorageStrategies.Roaming);
            _defcon3CheckList = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon3.json", StorageManagement.StorageStrategies.Roaming);
            _defcon4CheckList = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon4.json", StorageManagement.StorageStrategies.Roaming);
            _defcon5CheckList = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon5.json", StorageManagement.StorageStrategies.Roaming);

            if (_defcon1CheckList == null) _defcon1CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon2CheckList == null) _defcon2CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon3CheckList == null) _defcon3CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon4CheckList == null) _defcon4CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon5CheckList == null) _defcon5CheckList = new ItemObservableCollection<CheckListItem>();

            _defcon1CheckList.CollectionChanged += CollectionChanged;
            _defcon2CheckList.CollectionChanged += CollectionChanged;
            _defcon3CheckList.CollectionChanged += CollectionChanged;
            _defcon4CheckList.CollectionChanged += CollectionChanged;
            _defcon5CheckList.CollectionChanged += CollectionChanged;
        }        

        private async void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch ((sender as CheckListItem).DefconStatus)
            {
                case 1:
                    await StorageManagement.WriteFileAsync("defcon1.json", _defcon1CheckList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 2:
                    await StorageManagement.WriteFileAsync("defcon2.json", _defcon2CheckList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 3:
                    await StorageManagement.WriteFileAsync("defcon3.json", _defcon3CheckList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 4:
                    await StorageManagement.WriteFileAsync("defcon4.json", _defcon4CheckList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 5:
                    await StorageManagement.WriteFileAsync("defcon5.json", _defcon5CheckList, StorageManagement.StorageStrategies.Roaming);
                    break;
                default:
                    break;
            }
        }
        #region NotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName]string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public bool Set<T>(ref T storage, T value, [CallerMemberName]string propertyName = null)
        {
            if (Equals(storage, value))
                return false;
            storage = value;
            RaisePropertyChanged(propertyName);
            return true;
        }
        #endregion
    }
}
