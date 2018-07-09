using MyDEFCON_UWP.Models;
using System;
using System.Threading.Tasks;

namespace MyDEFCON_UWP.Services
{
    public static class CheckListService
    {
        public static async Task<ItemObservableCollection<CheckListItem>> LoadCheckList(int defcon)
        {
            var itemObservableCollection = new ItemObservableCollection<CheckListItem>();
            switch (defcon)
            {
                case 1:
                    itemObservableCollection = await StorageService.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon1.json", StorageService.StorageStrategies.Roaming);
                    break;
                case 2:
                    itemObservableCollection = await StorageService.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon2.json", StorageService.StorageStrategies.Roaming);
                    break;
                case 3:
                    itemObservableCollection = await StorageService.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon3.json", StorageService.StorageStrategies.Roaming);
                    break;
                case 4:
                    itemObservableCollection = await StorageService.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon4.json", StorageService.StorageStrategies.Roaming);
                    break;
                case 5:
                    itemObservableCollection = await StorageService.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon5.json", StorageService.StorageStrategies.Roaming);
                    break;
                default:
                    return itemObservableCollection;
            }
            if (itemObservableCollection != null) return itemObservableCollection;
            return new ItemObservableCollection<CheckListItem>();
        }

        public static async Task SaveCheckList(ItemObservableCollection<CheckListItem> checkList, int defcon)
        {
            switch (defcon)
            {
                case 1:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
                         StorageService.WriteFileAsync("defcon1.json", checkList, StorageService.StorageStrategies.Roaming);
                    break;
                case 2:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageService.WriteFileAsync("defcon2.json", checkList, StorageService.StorageStrategies.Roaming);
                    break;
                case 3:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageService.WriteFileAsync("defcon3.json", checkList, StorageService.StorageStrategies.Roaming);
                    break;
                case 4:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageService.WriteFileAsync("defcon4.json", checkList, StorageService.StorageStrategies.Roaming);
                    break;
                case 5:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageService.WriteFileAsync("defcon5.json", checkList, StorageService.StorageStrategies.Roaming);
                    break;
                default:
                    break;
            }
        }

        public static async Task<ItemObservableCollection<CheckListItem>> AddItemToCheckList(ItemObservableCollection<CheckListItem> checkList, CheckListItem item, int defcon)
        {
            checkList.Add(item);
            switch (defcon)
            {
                case 1:
                    await SaveCheckList(checkList, 1);
                    break;
                case 2:
                    await SaveCheckList(checkList, 2);
                    break;
                case 3:
                    await SaveCheckList(checkList, 3);
                    break;
                case 4:
                    await SaveCheckList(checkList, 4);
                    break;
                case 5:
                    await SaveCheckList(checkList, 5);
                    break;
                default:
                    break;
            }
            return checkList;
        }

        public static ItemObservableCollection<CheckListItem> RemoveItemFromCheckList(ItemObservableCollection<CheckListItem> checkList, CheckListItem item, int defcon)
        {
            return new ItemObservableCollection<CheckListItem>();
        }
    }
}
