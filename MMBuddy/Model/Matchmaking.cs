using MMBuddy.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMBuddy.Model
{
    class Matchmaking
    {
        public Matchmaking()
        {

        }

        public List<Champion> GetAllChampions()
        {
            var path = Directory.GetCurrentDirectory() + "\\Data\\championList.json";
            var rawData = File.ReadAllText(path);

            var championWrapperData = JsonConvert.DeserializeObject<ChampionWrapperList>(rawData);

            return championWrapperData.Data.Select(c => c.Value).ToList();
        }
    }
}
