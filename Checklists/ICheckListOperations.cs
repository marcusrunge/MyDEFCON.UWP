using Models;
using System.Threading.Tasks;

namespace Checklists
{
    public interface ICheckListOperations
    {
        Task SaveCheckList(ItemObservableCollection<CheckListItem> checkList, int defcon);

        Task SetDefconStatus(int status);

        Task ReverseUncheck(int defconStatus);
    }
}