using Models;
using Services;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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

        protected IChecklistCollection _checklistCollection;
        public IChecklistCollection Collection => _checklistCollection;

        protected ICheckListOperations _checkListOperations;
        public ICheckListOperations Operations => _checkListOperations;

        public async Task Initialize()
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
        }
    }
}
