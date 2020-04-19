using System.Threading.Tasks;

namespace Checklists
{
    public interface ICheckListOperations
    {
        Task Initialize();
        Task ReverseUncheck(int defconStatus);
    }
}
