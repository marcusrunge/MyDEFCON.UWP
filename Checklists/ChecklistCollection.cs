using Models;
using Services;
using System.ComponentModel;

namespace Checklists
{
    internal class ChecklistCollection : BindableBase, IChecklistCollection, INotifyPropertyChanged
    {
        private ChecklistsBase _checklistsBase;

        private static IChecklistCollection _checklistCollection;
        internal static IChecklistCollection Create(ChecklistsBase checklistsBase)
        {
            if (_checklistCollection == null) _checklistCollection = new ChecklistCollection(checklistsBase);
            return _checklistCollection;
        }

        public ChecklistCollection(ChecklistsBase checklistsBase)
        {
            _checklistsBase = checklistsBase;
        }

        public ItemObservableCollection<CheckListItem> Defcon1Checklist { get { return _checklistsBase._defcon1CheckList; } set { Set(ref _checklistsBase._defcon1CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon2Checklist { get { return _checklistsBase._defcon2CheckList; } set { Set(ref _checklistsBase._defcon2CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon3Checklist { get { return _checklistsBase._defcon3CheckList; } set { Set(ref _checklistsBase._defcon3CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon4Checklist { get { return _checklistsBase._defcon4CheckList; } set { Set(ref _checklistsBase._defcon4CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon5Checklist { get { return _checklistsBase._defcon5CheckList; } set { Set(ref _checklistsBase._defcon5CheckList, value); } }
        public ItemObservableCollection<CheckListItem> ActiveDefconCheckList { get { return _checklistsBase._activeDefconCheckList; } set { Set(ref _checklistsBase._activeDefconCheckList, value); } }
    }
}
