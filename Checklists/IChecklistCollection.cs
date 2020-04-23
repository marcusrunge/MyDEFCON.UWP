using Models;
using Services;
using System.ComponentModel;

namespace Checklists
{
    public interface IChecklistCollection: INotifyPropertyChanged
    {
        ItemObservableCollection<CheckListItem> Defcon1Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon2Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon3Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon4Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon5Checklist { get; set; }
        ItemObservableCollection<CheckListItem> ActiveDefconCheckList { get; set; }
    }
}
