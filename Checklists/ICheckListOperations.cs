using System.Threading.Tasks;

namespace Checklists
{
    public interface ICheckListOperations
    {        
        Task ReverseUncheck(int defconStatus);
    }
}
