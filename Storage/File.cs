using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public interface IFile
    {

    }
    internal class File : IFile
    {
        private static IFile _file;
        internal static IFile Create() => _file ?? (_file = new File());
    }
}
