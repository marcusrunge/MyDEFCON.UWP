using Storage;

namespace Checklists
{
    public static class ChecklistsFactory
    {
        private static IChecklists _checklists;
        public static IChecklists Create(/*IStorage storage*/) => _checklists ?? (_checklists = new Checklists(/*storage*/));
    }
}
