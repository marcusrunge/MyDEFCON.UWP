namespace Checklists
{
    internal class Checklists : ChecklistsBase
    {    
        internal Checklists(/*IStorage storage*/) : base(/*storage*/)
        {
            _checklistCollection = ChecklistCollection.Create(this);
            _checkListOperations = CheckListOperations.Create(this);
        }
    }
}
