namespace Storage
{
    public class StorageFactory
    {
        private static IStorage _storage;
        public static IStorage Create() => _storage ?? (_storage = new Storage());
    }
}
