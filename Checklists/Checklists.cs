namespace Checklists
{
    internal class Checklists : ChecklistsBase
    {
        internal Checklists()
        {
            _checklistCollection = ChecklistCollection.Create(this);
            _checkListOperations = CheckListOperations.Create(this);
        }
    }
}
