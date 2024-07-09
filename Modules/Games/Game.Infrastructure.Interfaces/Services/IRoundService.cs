using System.Collections.Generic;
using DataDomain.Rooms.Rounds;

namespace Game.Infrastructure.Interfaces.Services
{
    public interface IRoundService
    {
        List<RoundModel?>? CreateGameRounds();
    }
}