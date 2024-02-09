using System.Collections.Generic;
using System.Threading.Tasks;
using DataDomain.Rooms;

namespace Game.Infrastructure.Interfaces.Mangers
{
    public interface IGameManager
    {
        string CreateRoom();
        Task<bool> CloseRoom(string roomKey);
        IEnumerable<PlayerModel> GetPlayersFromRoom(string roomKey);
        PlayerModel GetHostPlayerFromRoom(string roomKey);
        GameModel? GetGame(string roomKey);
    }
}