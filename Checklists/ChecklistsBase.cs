using Models;
using Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Checklists
{   
    internal abstract class ChecklistsBase : INotifyPropertyChanged
    {
        protected ItemObservableCollection<CheckListItem> _defcon1CheckList;
        protected ItemObservableCollection<CheckListItem> _defcon2CheckList;
        protected ItemObservableCollection<CheckListItem> _defcon3CheckList;
        protected ItemObservableCollection<CheckListItem> _defcon4CheckList;
        protected ItemObservableCollection<CheckListItem> _defcon5CheckList;
        public ChecklistsBase()
        {
            _defcon1CheckList = new ItemObservableCollection<CheckListItem>();
            _defcon2CheckList = new ItemObservableCollection<CheckListItem>();
            _defcon3CheckList = new ItemObservableCollection<CheckListItem>();
            _defcon4CheckList = new ItemObservableCollection<CheckListItem>();
            _defcon5CheckList = new ItemObservableCollection<CheckListItem>();
            _defcon1CheckList.CollectionChanged += CollectionChanged;
            _defcon2CheckList.CollectionChanged += CollectionChanged;
            _defcon3CheckList.CollectionChanged += CollectionChanged;
            _defcon4CheckList.CollectionChanged += CollectionChanged;
            _defcon5CheckList.CollectionChanged += CollectionChanged;
        }

        private void CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch ((sender as CheckListItem).DefconStatus)
            {
                case 1:
                    break;
                case 2:
                    break;
                case 3:
                    break;
                case 4:
                    break;
                case 5:
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
