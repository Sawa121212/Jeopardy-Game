using DataDomain;
using DataDomain.Rooms;

namespace Game.Services
{
    public interface IRoomService
    {
        /// <summary>
        /// Создать комнату
        /// </summary>
        /// <returns></returns>
        string Create();

        /// <summary>
        /// Удалить комнату
        /// </summary>
        /// <param name="roomKey">Ключ комнаты</param>
        /// <returns></returns>
        bool Remove(string roomKey);

        /// <summary>
        /// Присоединить игрока к комнате
        /// </summary>
        /// <param name="roomKey">Ключ комнаты</param>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        bool ConnectPlayer(string roomKey, long playerId);

        /// <summary>
        /// Получить комнату по ключу
        /// </summary>
        /// <param name="roomKey">Ключ комнаты</param>
        /// <returns></returns>
        RoomModel? GetRoomByKey(string roomKey);

        /// <summary>
        /// Установить игрока "Ведущим"
        /// </summary>
        /// <param name="roomKey">Ключ комнаты</param>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        bool SetHost(string roomKey, long playerId);

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="roomKey"></param>
        /// <param name="playerId"></param>
        /// <returns></returns>
        bool KickPlayer(string roomKey, long playerId);

        /// <summary>
        /// Получить игру по ключу комнаты
        /// </summary>
        /// <param name="roomKey"></param>
        GameModel? GetGame(string roomKey);
    }
}