namespace Checklists
{
    public static class ChecklistsFactory
    {
        private static IChecklists _checklists;
        public static IChecklists Create() => _checklists ?? (_checklists = new Checklists());
    }
}
