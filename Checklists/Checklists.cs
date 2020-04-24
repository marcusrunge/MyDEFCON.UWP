using System.ComponentModel;
using System.Threading.Tasks;

namespace Checklists
{
    internal class Checklists : ChecklistsBase, IChecklists, INotifyPropertyChanged
    {
        public IChecklistCollection Collection => ChecklistCollection.Create();

        public ICheckListOperations Operations => CheckListOperations.Create();
        public async Task Initialize()
        {
            await InitializeBase();
        }
    }
}
