using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Storage
{
    public interface ISetting
    {

    }
    internal class Setting : ISetting
    {
        private static ISetting _setting;
        internal static ISetting Create() => _setting ?? (_setting = new Setting());
    }
}
