using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MMBuddy.Services;
using MMBuddy.Dtos;

namespace MMBuddy.Model
{
    public class Runes
    {
        private ServerInfo _serverInfo;

        public Runes()
        {
            // Try to establish a connection on construct
            this._serverInfo = LeagueProcess.Initialize();
        }

        /// <summary>
        /// Returns the current ServerInfo
        /// </summary>
        public ServerInfo ServerInfo
        {
            get { return this._serverInfo; }
        }

        /// <summary>
        /// Reinitializes the connection (if League was closed, for example)
        /// </summary>
        public void ReInitialize()
        {
            this._serverInfo = LeagueProcess.Initialize();
        }
    }
}
