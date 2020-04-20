using System.Threading.Tasks;

namespace Checklists
{
    public interface IChecklists
    {
        IChecklistCollection Collection { get; }
        ICheckListOperations Operations { get; }
        Task Initialize();
    }
}
