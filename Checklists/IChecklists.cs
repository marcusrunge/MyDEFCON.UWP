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
        IChecklistCollection Collection { get; }
        ICheckListOperations Operations { get; }
    }
}
