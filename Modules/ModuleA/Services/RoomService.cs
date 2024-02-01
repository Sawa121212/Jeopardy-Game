using System.Collections.ObjectModel;
using System.Linq;
using Common.Extensions;
using DataDomain;
using DataDomain.Rooms;
using Infrastructure.Domain.Helpers;
using Notification.Module.Services;
using ReactiveUI;
using Users.Infrastructure.Interfaces;

namespace Game.Services
{
    public class RoomService : ReactiveObject, IRoomService
    {
        public RoomService(INotificationService notificationService, IUserService userService)
        {
            _notificationService = notificationService;
            _userService = userService;
            Rooms = new ObservableCollection<RoomModel>();
        }

        /// <summary>
        /// Список комнат
        /// </summary>
        private ObservableCollection<RoomModel> Rooms
        {
            get => _rooms;
            init => this.RaiseAndSetIfChanged(ref _rooms, value);
        }

        /// <inheritdoc/>
        public string Create()
        {
            string key = RandomGenerator.GenerateRandomString(5);

            // сгенерировать уникальный ключ
            while (GetRoomByKey(key) != null)
            {
                key = RandomGenerator.GenerateRandomString(5);
            }

            RoomModel room = new(key);
            Rooms.Add(room);

            return room.Key;
        }

        /// <inheritdoc/>
        public bool ConnectPlayer(string roomKey, long playerId)
        {
            if (playerId == default)
            {
                return false;
            }

            RoomModel? room = GetRoomByKey(roomKey);
            if (room == null)
            {
                return false;
            }

            // get player from DB
            /*User user = _userService.GetUserById(playerId);
            if (user is null)
            {
                _notificationService.Show("Error", $"User '{playerId}' not found", NotificationType.Error);
                return false;
            }

            PlayerModel player = new(user);*/

            PlayerModel player = new() //Test
            {
                Id = playerId,
                Name = $"Test {playerId}"
            };

            room.Players.Add(player);

            return true;
        }

        /// <inheritdoc/>
        public bool Remove(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return false;
            }

            RoomModel? room = GetRoomByKey(roomKey);
            if (room == null)
            {
                return false;
            }

            return Rooms.Remove(room);
        }

        /// <inheritdoc/>
        public RoomModel? GetRoomByKey(string roomKey) => roomKey.IsNullOrEmpty() ? null : Rooms.FirstOrDefault(r => r.Key == roomKey);

        private PlayerModel? GetPlayerById(string playerId) => null;
        //playerId.IsNullOrEmpty() ? null : _userDbService.GetUserById(u => u.Id == playerId);

        /// <inheritdoc />
        public bool SetHost(string roomKey, long playerId)
        {
            if (playerId == default)
            {
                return false;
            }

            RoomModel? room = GetRoomByKey(roomKey);

            PlayerModel player = room?.Players.FirstOrDefault(e => e.Id == playerId);

            if (player == null)
            {
                return false;
            }

            if (room.Host != null)
            {
                room.Players.Add(room.Host);
            }

            room.Host = player;
            room.Players.Remove(player);

            return true;
        }

        /// <inheritdoc />
        public bool KickPlayer(string roomKey, long playerId)
        {
            if (playerId == default)
            {
                return false;
            }

            RoomModel? room = GetRoomByKey(roomKey);

            if (room == null)
            {
                return true;
            }

            PlayerModel player = room.Players.FirstOrDefault(e => e.Id == playerId);

            if (player == null)
            {
                if (room.Host != player)
                {
                    return false;
                }

                room.Host = null;
                return true;
            }

            room.Players.Remove(player);

            return true;
        }

        /// <inheritdoc />
        public GameModel? GetGame(string roomKey)
        {
            return GetRoomByKey(roomKey)?.Game;
        }

        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private ObservableCollection<RoomModel> _rooms;
    }
}