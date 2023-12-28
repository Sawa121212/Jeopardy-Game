using System.Collections.Generic;
using DataDomain;
using DataDomain.Rooms;

namespace Game.Mangers
{
    public interface IGameManager
    {
        string CreateRoom();
        IEnumerable<Player> GetPlayersFromRoom(string roomKey);
        Player GetHostPlayerFromRoom(string roomKey);
        DataDomain.Rooms.Game? GetGame(string roomKey);
    }
}