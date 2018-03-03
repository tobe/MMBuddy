namespace MMBuddy.Dtos
{
    public class RunePage
    {
        public bool current { get; set; }
        public bool isActive { get; set; }
        public string name { get; set; }
        public int primaryStyleId { get; set; }
        public int subStyleId { get; set; }
        public int[] selectedPerkIds { get; set; }
    }
}
