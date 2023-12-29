using System.Collections.Generic;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;

namespace DataDomain.Rooms
{
    public class Game
    {
        public Game()
        {
            CurrentRound = RoundsLevelEnum.Round1;
        }

        public RoundsLevelEnum CurrentRound { get; set; }
        public List<Round> Rounds { get; set; }

        public bool IsStarted { get; set; }
    }
}