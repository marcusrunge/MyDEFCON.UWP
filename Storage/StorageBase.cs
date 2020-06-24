namespace Storage
{
    internal class StorageBase : IStorage
    {
        protected ISetting _setting; 
        public ISetting Setting => _setting;

        protected IFile _file;
        public IFile File => _file;
    }
}
