using MMBuddy.Dtos;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MMBuddy.Model
{
    class Matchmaking
    {
        public Matchmaking()
        {

        }

        /// <summary>
        /// Returns all champions from a local JSON file.
        /// </summary>
        /// <returns>A List of Champions alphabetically ordered</returns>
        public List<Champion> GetAllChampions()
        {
            var path = Directory.GetCurrentDirectory() + "\\Data\\championList.json";
            var rawData = File.ReadAllText(path);

            var championWrapperData = JsonConvert.DeserializeObject<ChampionWrapperList>(rawData);

            return championWrapperData.Data.
                Select(c => c.Value).
                OrderBy(c => c.Name).
                ToList();
        }

        /// <summary>
        /// Processes the matchmaking, communicating with the server.
        /// </summary>
        /// <param name="Token">CancellationToken to cancel the async operation</param>
        /// <param name="Champion">The picked champion to lock</param>
        /// <param name="Lane">The lane to call</param>
        /// <returns></returns>
        public async Task ProcessMatchmaking(
                CancellationToken Token,
                Champion Champion,
                string Lane
            )
        {
            while(true)
            {
                Token.ThrowIfCancellationRequested();

                Debug.WriteLine("hi");

                await Task.Delay(200);
            }
        }
    }
}
