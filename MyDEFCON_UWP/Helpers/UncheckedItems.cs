using Models;
using Services;
using System;
using System.Linq;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace MyDEFCON_UWP.Helpers
{
    public static class UncheckedItems
    {
        public static int Count(ItemObservableCollection<CheckListItem> collection, int collectionID, int actualDefconStatus)
        {
            int i = 0;
            if (collectionID >= actualDefconStatus)
            {
                foreach (var item in collection)
                {
                    if (!item.Checked && !item.Deleted)
                    {
                        i++;
                    }
                }
                return i;
            }
            else return 0;
        }

        public static ItemObservableCollection<CheckListItem> Uncheck(ItemObservableCollection<CheckListItem> collection)
        {
            if (collection != null)
            {
                foreach (var item in collection)
                {
                    if(!item.Deleted) item.Checked = false;
                }
            }

            return collection;
        }

        public static int CountBadgeNumber(int actualDefconStatus, ItemObservableCollection<CheckListItem> defcon1CheckList, ItemObservableCollection<CheckListItem> defcon2CheckList, ItemObservableCollection<CheckListItem> defcon3CheckList, ItemObservableCollection<CheckListItem> defcon4CheckList, ItemObservableCollection<CheckListItem> defcon5CheckList)
        {
            int badgeNumber = 0;

            int defcon1UncheckedItems = 0;
            int defcon2UncheckedItems = 0;
            int defcon3UncheckedItems = 0;
            int defcon4UncheckedItems = 0;
            int defcon5UncheckedItems = 0;

            foreach (var item in defcon1CheckList)
            {
                if (!item.Checked && !item.Deleted)
                    defcon1UncheckedItems++;
            }

            foreach (var item in defcon2CheckList)
            {
                if (!item.Checked && !item.Deleted)
                    defcon2UncheckedItems++;
            }

            foreach (var item in defcon3CheckList)
            {
                if (!item.Checked && !item.Deleted)
                    defcon3UncheckedItems++;
            }

            foreach (var item in defcon4CheckList)
            {
                if (!item.Checked && !item.Deleted)
                    defcon4UncheckedItems++;
            }

            foreach (var item in defcon5CheckList)
            {
                if (!item.Checked && !item.Deleted)
                    defcon5UncheckedItems++;
            }

            switch (actualDefconStatus)
            {
                case 1:
                    badgeNumber = defcon1UncheckedItems + defcon2UncheckedItems + defcon3UncheckedItems + defcon4UncheckedItems + defcon5UncheckedItems;
                    break;
                case 2:
                    badgeNumber = defcon2UncheckedItems + defcon3UncheckedItems + defcon4UncheckedItems + defcon5UncheckedItems;
                    break;
                case 3:
                    badgeNumber = defcon3UncheckedItems + defcon4UncheckedItems + defcon5UncheckedItems;
                    break;
                case 4:
                    badgeNumber = defcon4UncheckedItems + defcon5UncheckedItems;
                    break;
                case 5:
                    badgeNumber = defcon5UncheckedItems;
                    break;
                default:
                    break;
            }

            return badgeNumber;
        }

        public static SolidColorBrush RectangleFill(ItemObservableCollection<CheckListItem> collection, int collectionID, int uncheckedItems, int actualDefconStatus)
        {
            if (collectionID >= actualDefconStatus)
            {
                try
                {
                    int validCollectionCount = 0;                    
                    foreach (var item in collection)
                    {
                        if (!item.Deleted) validCollectionCount++;
                    }
                    int deletedItemsCount = collection.Count - validCollectionCount;

                    if (deletedItemsCount != collection.Count && validCollectionCount == uncheckedItems && collection[0] != null)
                    {
                        return new SolidColorBrush(Colors.Red);
                    }
                    else if (validCollectionCount > uncheckedItems && uncheckedItems > 0)
                    {
                        return new SolidColorBrush(Colors.Orange);
                    }
                }

                catch (Exception)
                {
                    return new SolidColorBrush(Colors.Green);
                }
            }
            return new SolidColorBrush(Colors.Green);
        }
    }
}
