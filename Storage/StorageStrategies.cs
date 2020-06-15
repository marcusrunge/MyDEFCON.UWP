using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public enum StorageStrategies
    {
        /// <summary>Local, isolated folder</summary>
        Local,
        /// <summary>Cloud, isolated folder. 100k cumulative limit.</summary>
        Roaming,
        /// <summary>Local, temporary folder (not for settings)</summary>
        Temporary
    }
}
