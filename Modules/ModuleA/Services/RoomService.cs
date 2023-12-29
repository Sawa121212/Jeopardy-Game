using System.Collections.ObjectModel;
using System.Linq;
using Common.Extensions;
using DataDomain;
using DataDomain.Rooms;
using Infrastructure.Domain.Helpers;
using ReactiveUI;

namespace Game.Services
{
    public class RoomService : ReactiveObject, IRoomService
    {
        public RoomService()
        {
            Rooms = new ObservableCollection<Room>();
        }

        private ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set => this.RaiseAndSetIfChanged(ref _rooms, value);
        }

        public string Create()
        {
            string key = RandomGenerator.GenerateRandomString(5);

            while (GetByKey(key) != null)
            {
                key = RandomGenerator.GenerateRandomString(5);
            }

            Room room = new(key);
            Rooms.Add(room);

            return room.Key;
        }

        public bool ConnectPlayer(string roomKey, long playerId)
        {
            if (playerId == default)
            {
                return false;
            }

            Room? room = GetByKey(roomKey);
            if (room == null)
            {
                return false;
            }

            // ToDo: get player from DB
            Player player = new() //Test
            {
                Id = playerId,
                Name = $"Test {playerId}"
            };

            room.Players.Add(player);

            return true;
        }

        public bool Remove(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return false;
            }

            Room? room = GetByKey(roomKey);
            if (room == null)
            {
                return false;
            }

            return Rooms.Remove(room);
        }

        public Room? GetByKey(string roomKey) => roomKey.IsNullOrEmpty() ? null : Rooms.FirstOrDefault(r => r.Key == roomKey);

        private Player? GetPlayerById(string playerId) => null;
        //playerId.IsNullOrEmpty() ? null : _userDbService.GetUserById(u => u.Id == playerId);

        /// <inheritdoc />
        public bool SetHost(string roomKey, long playerId)
        {
            if (playerId == default)
            {
                return false;
            }

            Room? room = GetByKey(roomKey);

            Player player = room?.Players.FirstOrDefault(e => e.Id == playerId);

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

            Room? room = GetByKey(roomKey);

            if (room == null)
            {
                return true;
            }

            Player player = room.Players.FirstOrDefault(e => e.Id == playerId);

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
        public DataDomain.Rooms.Game? GetGame(string roomKey)
        {
            return GetByKey(roomKey)?.Game;
        }

        private ObservableCollection<Room> _rooms;
    }
}