using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Core.Components;
using DataDomain.Rooms;
using Telegram.Bot.Types;
using Users.Domain.Models;

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

        ///  <summary>
        ///  Присоединить игрока к комнате
        ///  </summary>
        ///  <param name="playerId"></param>
        Result TryConnectPlayerToRoom(long playerId);

        Result LeaveTheRoom(long userId);
    }
}