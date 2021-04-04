using Storage;

namespace Checklists
{
    public interface IChecklistsFactory
    {
        IChecklists Create();
    }

    public class ChecklistsFactory : IChecklistsFactory
    {
        private readonly IStorage _storage;
        public ChecklistsFactory(IStorage storage) => _storage = storage;
        private static IChecklists _checklists;
        public IChecklists Create() => _checklists ?? (_checklists = new Checklists(_storage));
    }
}
