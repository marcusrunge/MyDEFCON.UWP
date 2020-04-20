using Models;
using Services;

namespace Checklists
{
    public interface IChecklistCollection
    {
        ItemObservableCollection<CheckListItem> Defcon1Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon2Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon3Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon4Checklist { get; set; }
        ItemObservableCollection<CheckListItem> Defcon5Checklist { get; set; }
    }
}
