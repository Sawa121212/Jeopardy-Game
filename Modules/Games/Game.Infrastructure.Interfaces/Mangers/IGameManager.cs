using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Core.Components;
using DataDomain.Rooms;

namespace Game.Infrastructure.Interfaces.Mangers
{
    public interface IGameManager
    {
        bool CreateRoom();
        Task<bool> CloseRoom();
        IEnumerable<PlayerModel> GetPlayersFromRoom();
        PlayerModel GetHostPlayerFromRoom();
        GameModel? GetGame();
        bool CloseGame();

        Result SetPlayerToHost(long playerId);
        Result GetOutHostPlayer();

        ///  <summary>
        ///  Присоединить игрока к комнате
        ///  </summary>
        ///  <param name="playerId"></param>
        Result TryConnectPlayerToRoom(long playerId);

        Result LeaveTheRoom(long userId);

        /// <summary>
        /// Добавить бота
        /// </summary>
        /// <returns></returns>
        bool AddBot();
    }
}