using System;
using System.Threading.Tasks;

namespace Checklists
{
    internal class CheckListOperations : ChecklistsBase, ICheckListOperations
    {
        private static ICheckListOperations _checkListOperations;
        internal static ICheckListOperations Create() 
        {
            if (_checkListOperations == null) _checkListOperations = new CheckListOperations();
            return _checkListOperations;
        }

        public Task ReverseUncheck(int defconStatus)
        {
            throw new NotImplementedException();
        }
    }
}
