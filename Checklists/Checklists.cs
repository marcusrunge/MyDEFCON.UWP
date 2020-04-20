namespace Checklists
{
    internal class Checklists : IChecklists
    {
        public IChecklistCollection Collection => ChecklistCollection.Create();

        public ICheckListOperations Operations => CheckListOperations.Create();
    }
}
