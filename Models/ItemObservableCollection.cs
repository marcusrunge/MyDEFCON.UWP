using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Models
{
    public class ItemObservableCollection<T> : ObservableCollection<T> where T : INotifyPropertyChanged
    {
        #region Constructor

        public ItemObservableCollection()
        {
            CollectionChanged += ItemObservableCollection_CollectionChanged;
        }

        #endregion Constructor

        #region Eventhandler

        private void ItemObservableCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged += ItemObservableCollection_PropertyChanged;
                }
            }
            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged)item).PropertyChanged -= ItemObservableCollection_PropertyChanged;
                }
            }
        }

        private void ItemObservableCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            NotifyCollectionChangedEventArgs args = new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, sender, sender, IndexOf((T)sender));
            OnCollectionChanged(args);
        }

        public static implicit operator ItemObservableCollection<T>(ItemObservableCollection<CheckListItem> v)
        {
            throw new NotImplementedException();
        }

        #endregion Eventhandler
    }
}