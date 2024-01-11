using System.Collections.Generic;
using DataDomain.Rooms;

namespace Game.Mangers
{
    public interface IGameManager
    {
        string CreateRoom();
        IEnumerable<PlayerModel> GetPlayersFromRoom(string roomKey);
        PlayerModel GetHostPlayerFromRoom(string roomKey);
        GameModel? GetGame(string roomKey);
    }
}