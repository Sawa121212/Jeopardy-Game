using System.Threading.Tasks;
using Common.Core.Components;
using DataDomain;
using DataDomain.Rooms;

namespace Game.Infrastructure.Interfaces.Services
{
    public interface IRoomService
    {
        /// <summary>
        /// Создать комнату
        /// </summary>
        /// <returns></returns>
        Result Create();

        /// <summary>
        /// Удалить комнату
        /// </summary>
        /// <returns></returns>
        Task<Result> Remove();

        /// <summary>
        /// Получить комнату
        /// </summary>
        /// <returns></returns>
        RoomModel? GetRoom();

        /// <summary>
        /// Получить комнату
        /// </summary>
        /// <returns></returns>
        Result<RoomModel> TryGetRoom();

        /// <summary>
        /// Присоединить игрока к комнате
        /// </summary>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        Result ConnectPlayer(long playerId);

        /// <summary>
        /// Установить игрока "Ведущим"
        /// </summary>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        Result SetHost(long playerId);

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<Result> KickPlayer(long playerId);

        Result LeaveTheRoom(long userId);
    }
}