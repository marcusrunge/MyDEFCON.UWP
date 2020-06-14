using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;

namespace Storage
{
    public class StorageFactory
    {
        private static IStorage _storage;
        public static IStorage Create() => _storage ?? (_storage = new Storage());
    }
}
