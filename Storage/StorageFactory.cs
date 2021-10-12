namespace Storage
{
    public interface IStorageFactory
    {
        IStorage Create();
    }

    public class StorageFactory : IStorageFactory
    {
        private static IStorage _storage;

        public IStorage Create() => _storage ?? (_storage = new Storage());
    }
}