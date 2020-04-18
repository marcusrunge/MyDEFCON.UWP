using Models;
using System.Threading.Tasks;

namespace Services
{
    public static class CheckListManagement
    {
        public static async Task<ItemObservableCollection<CheckListItem>> LoadCheckList(int defcon)
        {
            var itemObservableCollection = new ItemObservableCollection<CheckListItem>();
            switch (defcon)
            {
                case 1:
                    itemObservableCollection = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon1.json", StorageManagement.StorageStrategies.Roaming);
                    break;
                case 2:
                    itemObservableCollection = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon2.json", StorageManagement.StorageStrategies.Roaming);
                    break;
                case 3:
                    itemObservableCollection = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon3.json", StorageManagement.StorageStrategies.Roaming);
                    break;
                case 4:
                    itemObservableCollection = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon4.json", StorageManagement.StorageStrategies.Roaming);
                    break;
                case 5:
                    itemObservableCollection = await StorageManagement.ReadFileAsync<ItemObservableCollection<CheckListItem>>("defcon5.json", StorageManagement.StorageStrategies.Roaming);
                    break;
                default:
                    break;
            }
            /*if (itemObservableCollection != null && filterDeletedItems)
            {
                for (int i = 0; i < itemObservableCollection.Count; i++)
                {
                    if (itemObservableCollection[i].Deleted) itemObservableCollection.RemoveAt(i);
                }
            }*/
            if (itemObservableCollection != null) return itemObservableCollection;
            else return new ItemObservableCollection<CheckListItem>();
        }

        public static async Task SaveCheckList(ItemObservableCollection<CheckListItem> checkList, int defcon)
        {
            switch (defcon)
            {
                case 1:
                    await
                         //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
                         StorageManagement.WriteFileAsync("defcon1.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 2:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageManagement.WriteFileAsync("defcon2.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 3:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageManagement.WriteFileAsync("defcon3.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 4:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageManagement.WriteFileAsync("defcon4.json", checkList, StorageManagement.StorageStrategies.Roaming);
                    break;
                case 5:
                    await
                        //Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                        StorageManagement.WriteFileAsync("defcon5.json", checkList, StorageManagement.StorageStrategies.Roaming);
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
