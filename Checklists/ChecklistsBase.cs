using Models;
using Storage;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Checklists
{
    internal abstract class ChecklistsBase : BindableBase, IChecklists, INotifyPropertyChanged
    {
        internal protected ItemObservableCollection<CheckListItem> _defcon1CheckList;
        internal protected ItemObservableCollection<CheckListItem> _defcon2CheckList;
        internal protected ItemObservableCollection<CheckListItem> _defcon3CheckList;
        internal protected ItemObservableCollection<CheckListItem> _defcon4CheckList;
        internal protected ItemObservableCollection<CheckListItem> _defcon5CheckList;
        internal protected ItemObservableCollection<CheckListItem> _activeDefconCheckList;
        internal protected int _defconStatus;
        internal protected IStorage _storage;

        protected IChecklistCollection _checklistCollection;
        public IChecklistCollection Collection => _checklistCollection;

        protected ICheckListOperations _checkListOperations;
        public ICheckListOperations Operations => _checkListOperations;

        public ChecklistsBase(/*IStorage storage*/)
        {
            _storage = StorageFactory.Create();
        }

        public async Task Initialize()
        {
            _defcon1CheckList = await _storage.File.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon1.json", StorageStrategies.Roaming);
            _defcon2CheckList = await _storage.File.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon2.json", StorageStrategies.Roaming);
            _defcon3CheckList = await _storage.File.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon3.json", StorageStrategies.Roaming);
            _defcon4CheckList = await _storage.File.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon4.json", StorageStrategies.Roaming);
            _defcon5CheckList = await _storage.File.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon5.json", StorageStrategies.Roaming);

            if (_defcon1CheckList == null) _defcon1CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon2CheckList == null) _defcon2CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon3CheckList == null) _defcon3CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon4CheckList == null) _defcon4CheckList = new ItemObservableCollection<CheckListItem>();
            if (_defcon5CheckList == null) _defcon5CheckList = new ItemObservableCollection<CheckListItem>();
        }
    }
}
