using System.Collections.Generic;

namespace MMBuddy.Dtos
{
    public class ChampionWrapperList
    {
        public string Type { get; set; }
        public string Version { get; set; }
        public Dictionary<string, Champion> Data { get; set; }
    }

    public class Champion
    {
        public string Title { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
