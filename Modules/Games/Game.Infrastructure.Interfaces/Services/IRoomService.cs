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
        bool Create();

        /// <summary>
        /// Удалить комнату
        /// </summary>
        /// <returns></returns>
        Task<bool> Remove();

        /// <summary>
        /// Присоединить игрока к комнате
        /// </summary>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        bool ConnectPlayer(long playerId);

        /// <summary>
        /// Добавить бота
        /// </summary>
        /// <returns></returns>
        public bool AddBot();

        /// <summary>
        /// Получить комнату
        /// </summary>
        /// <returns></returns>
        RoomModel? GetRoom();

        /// <summary>
        /// Установить игрока "Ведущим"
        /// </summary>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        bool SetHost(long playerId);

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        Task<bool> KickPlayer(long playerId);

        /// <summary>
        /// Получить игру по ключу комнаты
        /// </summary>
        GameModel? GetGame();

        Result LeaveTheRoom(long userId);
    }
}