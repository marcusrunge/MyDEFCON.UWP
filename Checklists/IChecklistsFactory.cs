using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Checklists
{
    public static class ChecklistsFactory 
    {
        public static IChecklists Create()
        {
            return new Checklists();
        }
    }
}
