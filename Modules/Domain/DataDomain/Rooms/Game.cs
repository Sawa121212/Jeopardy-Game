using System.Collections.Generic;
using DataDomain.Rooms.Rounds;

namespace DataDomain.Rooms
{
    public class Game
    {
        public List<Round> Rounds { get; set; }

        public bool IsStarted { get; set; }
    }
}