using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklists
{
    public interface IChecklistsFactory
    {
        IChecklists Create();
    }

    public class ChecklistsFactory : IChecklistsFactory
    {
        public IChecklists Create()
        {
            return new Checklists();
        }
    }
}
