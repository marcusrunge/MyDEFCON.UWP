namespace Storage
{
    public interface IStorage
    {
        ISetting Setting { get; }
        IFile File { get; }
    }
    internal class Storage : StorageBase
    {
        public Storage()
        {
            _file = global::Storage.File.Create();
            _setting = global::Storage.Setting.Create();
        }
    }
}
