using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklists
{
    internal class CheckListOperations : ICheckListOperations
    {
        private static ICheckListOperations _checkListOperations;
        internal static ICheckListOperations Create() 
        {
            if (_checkListOperations == null) _checkListOperations = new CheckListOperations();
            return _checkListOperations;
        }

        public Task Initialize()
        {
            throw new NotImplementedException();
        }

        public Task ReverseUncheck(int defconStatus)
        {
            throw new NotImplementedException();
        }
    }
}
