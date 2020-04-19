using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
