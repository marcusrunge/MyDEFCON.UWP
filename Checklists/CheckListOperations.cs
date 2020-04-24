using Models;
using Services;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Checklists
{
    internal class CheckListOperations : ChecklistsBase, ICheckListOperations
    {
        private static ICheckListOperations _checkListOperations;
        internal static ICheckListOperations Create()
        {
            if (_checkListOperations == null) _checkListOperations = new CheckListOperations();
            return _checkListOperations;
        }

        public async Task ReverseUncheck(int defconStatus)
        {
            switch (defconStatus)
            {
                case 2:
                    if (_defconStatus != 2)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        await SaveCheckList(_defcon1CheckList, 1);
                    }

                    break;
                case 3:
                    if (_defconStatus != 3)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        _defcon2CheckList = UncheckCollection(_defcon2CheckList);
                        await SaveCheckList(_defcon1CheckList, 1);
                        await SaveCheckList(_defcon2CheckList, 2);
                    }

                    break;
                case 4:
                    if (_defconStatus != 4)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        _defcon2CheckList = UncheckCollection(_defcon2CheckList);
                        _defcon3CheckList = UncheckCollection(_defcon3CheckList);
                        await SaveCheckList(_defcon1CheckList, 1);
                        await SaveCheckList(_defcon2CheckList, 2);
                        await SaveCheckList(_defcon3CheckList, 3);
                    }

                    break;
                case 5:
                    if (_defconStatus != 5)
                    {
                        _defcon1CheckList = UncheckCollection(_defcon1CheckList);
                        _defcon2CheckList = UncheckCollection(_defcon2CheckList);
                        _defcon3CheckList = UncheckCollection(_defcon3CheckList);
                        _defcon4CheckList = UncheckCollection(_defcon4CheckList);
                        await SaveCheckList(_defcon1CheckList, 1);
                        await SaveCheckList(_defcon2CheckList, 2);
                        await SaveCheckList(_defcon3CheckList, 3);
                        await SaveCheckList(_defcon4CheckList, 4);
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

        public async Task SaveCheckList(ItemObservableCollection<CheckListItem> checkList, int defcon)
        {
            switch (defcon)
            {
                case 1:
                    await StorageManagement.WriteFileAsync("defcon1.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    _defcon1CheckList = checkList;
                    break;
                case 2:
                    await StorageManagement.WriteFileAsync("defcon2.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    _defcon2CheckList = checkList;
                    break;
                case 3:
                    await StorageManagement.WriteFileAsync("defcon3.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    _defcon3CheckList = checkList;
                    break;
                case 4:
                    await StorageManagement.WriteFileAsync("defcon4.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    _defcon4CheckList = checkList;
                    break;
                case 5:
                    await StorageManagement.WriteFileAsync("defcon5.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    _defcon5CheckList = checkList;
                    break;
                default:
                    break;
            }
        }

        public async Task SetDefconStatus(int status)
        {
            if(_activeDefconCheckList!=null) _activeDefconCheckList.CollectionChanged -= CollectionChanged;
            _activeDefconCheckList = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>($"defcon{status}.json", StorageManagement.StorageStrategies.Roaming);
            if (_activeDefconCheckList == null) _activeDefconCheckList = new ItemObservableCollection<CheckListItem>();
            _defconStatus = status;
            //_activeDefconCheckList.CollectionChanged += CollectionChanged;
        }

        private async void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await StorageManagement.WriteFileAsync($"defcon{_defconStatus}.json", _activeDefconCheckList, StorageManagement.StorageStrategies.Roaming);
            switch (_defconStatus)
            {
                case 1:
                    _defcon1CheckList = _activeDefconCheckList;
                    break;
                case 2:
                    _defcon2CheckList = _activeDefconCheckList;
                    break;
                case 3:
                    _defcon3CheckList = _activeDefconCheckList;
                    break;
                case 4:
                    _defcon4CheckList = _activeDefconCheckList;
                    break;
                case 5:
                    _defcon5CheckList = _activeDefconCheckList;
                    break;
                default:
                    break;
            }
        }
    }
}
