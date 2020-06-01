using System;
using System.Collections.Generic;
using System.Text;

namespace MyDEFCON_UWP.Core.Eventaggregator
{
    public interface IEventAggregator
    {
        ISubscribe Subscribe { get; }
        IPublish Publish { get; }
    }
}
