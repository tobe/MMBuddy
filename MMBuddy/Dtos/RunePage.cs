namespace MMBuddy.Dtos
{
    public class RunePage
    {
        public bool Current { get; set; }
        public bool IsActive { get; set; }
        public string Name { get; set; }
        public int PrimaryStyleId { get; set; }
        public int SubStyleId { get; set; }
        public int[] SelectedPerkIds { get; set; }
    }
}
