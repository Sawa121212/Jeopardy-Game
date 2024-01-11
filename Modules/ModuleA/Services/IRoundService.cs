using System.Collections.Generic;
using DataDomain.Rooms.Rounds;

namespace Game.Services
{
    public interface IRoundService
    {
        List<RoundModel?>? CreateGameRounds();
    }
}