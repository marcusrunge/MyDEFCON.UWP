using System.Threading.Tasks;

namespace Checklists
{
    internal class Checklists : ChecklistsBase, IChecklists
    {
        public IChecklistCollection Collection => ChecklistCollection.Create();

        public ICheckListOperations Operations => CheckListOperations.Create();
        public async Task Initialize()
        {
            await InitializeBase();
        }
    }
}
