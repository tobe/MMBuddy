using System.Threading.Tasks;
using MMBuddy.Services;
using MMBuddy.Dtos;
using System.Collections.Generic;

namespace MMBuddy.Model
{
    public class Runes
    {
        private readonly Preferences _preferences;

        public Runes()
        {
            // Try to establish a connection on construct
            this.ServerInfo = LeagueProcess.Initialize();

            // Load preferences
            this._preferences = new Preferences();
        }

        /// <summary>
        /// Returns the current ServerInfo
        /// </summary>
        public ServerInfo ServerInfo { get; private set; }

        /// <summary>
        /// Reinitializes the connection (if League was closed, for example)
        /// </summary>
        public void ReInitialize()
        {
            this.ServerInfo = LeagueProcess.Initialize();
        }

        /// <summary>
        /// Returns the current rune page.
        /// </summary>
        /// <returns></returns>
        public async Task<RunePage> GetCurrentRunePageAsync()
        {
            return await ApiClient.GetCurrentRunePage();
        }

        /// <summary>
        /// Returns the saved rune page count.
        /// </summary>
        /// <returns></returns>
        public bool RunePagesExist()
        {
            return this._preferences.Data != null;
        }

        /// <summary>
        /// Returns saved rune pages.
        /// </summary>
        /// <returns></returns>
        public List<RunePage> GetSavedRunePages()
        {
            return this._preferences.Data;
        }
    }
}
