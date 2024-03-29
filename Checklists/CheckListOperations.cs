﻿using Models;
using Storage;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Checklists
{
    internal class CheckListOperations : ICheckListOperations
    {
        private readonly ChecklistsBase _checklistsBase;

        private static ICheckListOperations _checkListOperations;

        internal static ICheckListOperations Create(ChecklistsBase checklistsBase) => _checkListOperations ?? (_checkListOperations = new CheckListOperations(checklistsBase));

        internal CheckListOperations(ChecklistsBase checklistsBase)
        {
            _checklistsBase = checklistsBase;
        }

        public async Task ReverseUncheck(int defconStatus)
        {
            switch (defconStatus)
            {
                case 2:
                    if (_checklistsBase._defconStatus != 2)
                    {
                        _checklistsBase._defcon1CheckList = UncheckCollection(_checklistsBase._defcon1CheckList);
                        await SaveCheckList(_checklistsBase._defcon1CheckList, 1);
                    }

                    break;

                case 3:
                    if (_checklistsBase._defconStatus != 3)
                    {
                        _checklistsBase._defcon1CheckList = UncheckCollection(_checklistsBase._defcon1CheckList);
                        _checklistsBase._defcon2CheckList = UncheckCollection(_checklistsBase._defcon2CheckList);
                        await SaveCheckList(_checklistsBase._defcon1CheckList, 1);
                        await SaveCheckList(_checklistsBase._defcon2CheckList, 2);
                    }

                    break;

                case 4:
                    if (_checklistsBase._defconStatus != 4)
                    {
                        _checklistsBase._defcon1CheckList = UncheckCollection(_checklistsBase._defcon1CheckList);
                        _checklistsBase._defcon2CheckList = UncheckCollection(_checklistsBase._defcon2CheckList);
                        _checklistsBase._defcon3CheckList = UncheckCollection(_checklistsBase._defcon3CheckList);
                        await SaveCheckList(_checklistsBase._defcon1CheckList, 1);
                        await SaveCheckList(_checklistsBase._defcon2CheckList, 2);
                        await SaveCheckList(_checklistsBase._defcon3CheckList, 3);
                    }

                    break;

                case 5:
                    if (_checklistsBase._defconStatus != 5)
                    {
                        _checklistsBase._defcon1CheckList = UncheckCollection(_checklistsBase._defcon1CheckList);
                        _checklistsBase._defcon2CheckList = UncheckCollection(_checklistsBase._defcon2CheckList);
                        _checklistsBase._defcon3CheckList = UncheckCollection(_checklistsBase._defcon3CheckList);
                        _checklistsBase._defcon4CheckList = UncheckCollection(_checklistsBase._defcon4CheckList);
                        await SaveCheckList(_checklistsBase._defcon1CheckList, 1);
                        await SaveCheckList(_checklistsBase._defcon2CheckList, 2);
                        await SaveCheckList(_checklistsBase._defcon3CheckList, 3);
                        await SaveCheckList(_checklistsBase._defcon4CheckList, 4);
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
                    await _checklistsBase._storage.File.WriteFileAsync("defcon1.json", checkList, StorageStrategies.Roaming);
                    _checklistsBase._defcon1CheckList = checkList;
                    break;

                case 2:
                    await _checklistsBase._storage.File.WriteFileAsync("defcon2.json", checkList, StorageStrategies.Roaming);
                    _checklistsBase._defcon2CheckList = checkList;
                    break;

                case 3:
                    await _checklistsBase._storage.File.WriteFileAsync("defcon3.json", checkList, StorageStrategies.Roaming);
                    _checklistsBase._defcon3CheckList = checkList;
                    break;

                case 4:
                    await _checklistsBase._storage.File.WriteFileAsync("defcon4.json", checkList, StorageStrategies.Roaming);
                    _checklistsBase._defcon4CheckList = checkList;
                    break;

                case 5:
                    await _checklistsBase._storage.File.WriteFileAsync("defcon5.json", checkList, StorageStrategies.Roaming);
                    _checklistsBase._defcon5CheckList = checkList;
                    break;

                default:
                    break;
            }
        }

        public async Task SetDefconStatus(int status)
        {
            if (_checklistsBase._activeDefconCheckList != null) _checklistsBase._activeDefconCheckList.CollectionChanged -= CollectionChanged;
            _checklistsBase._activeDefconCheckList = await _checklistsBase._storage.File.ReadFileAsync<ItemObservableCollection<CheckListItem>>($"defcon{status}.json", StorageStrategies.Roaming);
            if (_checklistsBase._activeDefconCheckList == null) _checklistsBase._activeDefconCheckList = new ItemObservableCollection<CheckListItem>();
            _checklistsBase._defconStatus = status;
            //_activeDefconCheckList.CollectionChanged += CollectionChanged;
        }

        private async void CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            await _checklistsBase._storage.File.WriteFileAsync($"defcon{_checklistsBase._defconStatus}.json", _checklistsBase._activeDefconCheckList, StorageStrategies.Roaming);
            switch (_checklistsBase._defconStatus)
            {
                case 1:
                    _checklistsBase._defcon1CheckList = _checklistsBase._activeDefconCheckList;
                    break;

                case 2:
                    _checklistsBase._defcon2CheckList = _checklistsBase._activeDefconCheckList;
                    break;

                case 3:
                    _checklistsBase._defcon3CheckList = _checklistsBase._activeDefconCheckList;
                    break;

                case 4:
                    _checklistsBase._defcon4CheckList = _checklistsBase._activeDefconCheckList;
                    break;

                case 5:
                    _checklistsBase._defcon5CheckList = _checklistsBase._activeDefconCheckList;
                    break;

                default:
                    break;
            }
        }
    }
}