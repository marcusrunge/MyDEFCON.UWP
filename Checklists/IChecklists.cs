using Models;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Checklists
{
    public interface IChecklists
    {
        IChecklistCollection ChecklistCollection { get; }
        ICheckListOperations CheckListOperations { get; }
    }
}
