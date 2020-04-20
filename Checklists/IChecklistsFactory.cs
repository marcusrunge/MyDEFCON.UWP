namespace Checklists
{
    public static class ChecklistsFactory 
    {
        public static IChecklists Create()
        {
            return new Checklists();
        }
    }
}
