using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMBuddy.Dtos
{
    /* Thanks: https://github.com/molenzwiebel/Mimic/blob/master/web/src/components/champ-select/champ-select.ts */

    public class Action
    {
        public int ActorCellId { get; set; }
        public int ChampionId { get; set; }
        public bool Completed { get; set; }
        public int Id { get; set; }
        public int PickTurn { get; set; }
        public string Type { get; set; }
    }

    public class ChatDetails
    {
        public string ChatRoomName { get; set; }
        public string ChatRoomPassword { get; set; }
    }

    public class Player
    {
        public int CellId { get; set; }
        public int ChampionId { get; set; }
        public string DisplayName { get; set; }
    }

    public class Session
    {
        public List<List<Action>> Actions { get; set; }
        public ChatDetails ChatDetails { get; set; }
        public int LocalPlayerCellId { get; set; }
        public List<Player> MyTeam { get; set; }
    }
}
