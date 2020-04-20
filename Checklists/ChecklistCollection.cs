using Models;
using Services;

namespace Checklists
{
    internal class ChecklistCollection : ChecklistsBase, IChecklistCollection
    {
        private static IChecklistCollection _checklistCollection;
        internal static IChecklistCollection Create()
        {
            if (_checklistCollection == null) _checklistCollection = new ChecklistCollection();
            return _checklistCollection;
        }

        public ItemObservableCollection<CheckListItem> Defcon1Checklist { get { return _defcon1CheckList; } set { Set(ref _defcon1CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon2Checklist { get { return _defcon2CheckList; } set { Set(ref _defcon2CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon3Checklist { get { return _defcon3CheckList; } set { Set(ref _defcon3CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon4Checklist { get { return _defcon4CheckList; } set { Set(ref _defcon4CheckList, value); } }
        public ItemObservableCollection<CheckListItem> Defcon5Checklist { get { return _defcon5CheckList; } set { Set(ref _defcon5CheckList, value); } }
    }
}
