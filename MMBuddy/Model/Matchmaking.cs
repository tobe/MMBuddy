using MMBuddy.Dtos;
using MMBuddy.Services;
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

                // Get the current session
                var currentSession = await ApiClient.GetSession();
                if(currentSession == null)
                {
                    // Session doesn't exist, sleep and continue
                    await Task.Delay(100);
                    continue;
                }

                // Grab the local player ID
                var localPlayerId = this.GetLocalPlayerId(currentSession);
                if(localPlayerId == null)
                {
                    await Task.Delay(100);
                    continue;
                }

                // Hover / Lock
                var result = await ApiClient.Hover(
                    localPlayerId,
                    Int32.Parse(Champion.Id),
                    true
                );

                // Send a few messages
                for (int i = 0; i < 4; i++)
                {
                    ApiClient.SendChatMessage(currentSession.ChatDetails.ChatRoomName, Lane);
                    await Task.Delay(100);
                }

                if (result)
                    return;

                await Task.Delay(5000);
            }
        }

        /// <summary>
        /// Returns the local player ID from a matchmaking session
        /// </summary>
        /// <param name="Session">The session</param>
        /// <returns>Local player ID</returns>
        private int? GetLocalPlayerId(Session Session)
        {
            var localPlayer = Session.MyTeam.Where(p => p.CellId == Session.LocalPlayerCellId).SingleOrDefault();
            if (localPlayer == null)
                return null;

            return Session.Actions[0] // always a single array of arrays for blind pick
                .Where(a => a.ActorCellId == localPlayer.CellId)
                .SingleOrDefault()
                .Id;
        }
    }
}
