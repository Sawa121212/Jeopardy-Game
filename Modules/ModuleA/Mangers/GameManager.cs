using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Notifications;
using Common.Extensions;
using DataDomain;
using DataDomain.Rooms;
using Game.Events;
using Game.Services;
using Notification.Module.Services;
using Prism.Events;
using ReactiveUI;

namespace Game.Mangers
{
    public class GameManager : ReactiveObject, IGameManager
    {
        public GameManager(
            IEventAggregator eventAggregator,
            INotificationService notificationService,
            IRoomService roomService, IRoundService roundService)
        {
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _roomService = roomService;
            _roundService = roundService;
            _eventAggregator.GetEvent<PlayerIsTryingToConnectToRoomEvent>().Subscribe(e => ConnectPlayerToRoom(e.RoomKey, e.PlayerId));
            _eventAggregator.GetEvent<SetPlayerToHostEvent>().Subscribe(e => SetPlayerToHost(e.RoomKey, e.PlayerId));
            _eventAggregator.GetEvent<KickOutPlayerEvent>().Subscribe(e => KickOutPlayer(e.RoomKey, e.PlayerId));
            _eventAggregator.GetEvent<GameIsReadyToStartEvent>().Subscribe(e => StartGame(e.RoomKey));
        }

        /// <inheritdoc />
        public string CreateRoom()
        {
            return _roomService.Create();
        }

        private bool StartGame(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return false;
            }

            RoomModel? room = _roomService.GetRoomByKey(roomKey);

            if (room != null)
            {
                room.Game = new GameModel
                {
                    Rounds = _roundService.CreateGameRounds()
                };

                if (room.Game.Rounds != null)
                {
                    room.Game.IsStarted = room.Game.Rounds.Count > 0;
                }

                _eventAggregator.GetEvent<GameIsStartedEvent>().Publish(new GameIsStartedEvent(roomKey));
            }
            else
            {
                return false;
            }

            return true;
        }

        /// <inheritdoc />
        public IEnumerable<PlayerModel> GetPlayersFromRoom(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return null;
            }

            return _roomService.GetRoomByKey(roomKey)?.Players;
        }

        /// <inheritdoc />
        public PlayerModel GetHostPlayerFromRoom(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return null;
            }

            return _roomService.GetRoomByKey(roomKey)?.Host;
        }

        /// <inheritdoc />
        public GameModel? GetGame(string roomKey)
        {
            return _roomService.GetGame(roomKey);
        }

        /// <inheritdoc />
        public bool RemoveRoom(string key)
        {
            if (key.IsNullOrEmpty())
            {
                return false;
            }

            return _roomService.Remove(key);
        }

        /// <summary>
        /// Присоединить игрока к комнате
        /// </summary>
        /// <param name="roomKey"></param>
        /// <param name="playerId"></param>
        private void ConnectPlayerToRoom(string roomKey, long playerId)
        {
            if (roomKey.IsNullOrEmpty() || playerId == default)
            {
                return;
            }

            RoomModel? room = _roomService.GetRoomByKey(roomKey);
            if (room is null)
            {
                return;
            }

            if (_roomService.ConnectPlayer(roomKey, playerId))
            {
                _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Publish(new NumberOfPlayersInRoomIsUpdatedEvent(roomKey));
            }
            else
            {
                _notificationService.Show("Ошибка", $"В комнату игрок не смог присоединится", NotificationType.Error);
            }
        }

        /// <summary>
        /// Установить игрока в качестве организатора
        /// </summary>
        /// <param name="roomKey"></param>
        /// <param name="playerId"></param>
        private void SetPlayerToHost(string roomKey, long playerId)
        {
            if (_roomService.SetHost(roomKey, playerId))
            {
                _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Publish(new HostPlayerUpdatedEvent(roomKey));
            }
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="roomKey"></param>
        /// <param name="playerId"></param>
        private void KickOutPlayer(string roomKey, long playerId)
        {
            if (_roomService.KickPlayer(roomKey, playerId))
            {
                _eventAggregator.GetEvent<PlayerKickedOutEvent>().Publish(new PlayerKickedOutEvent(roomKey));
            }
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;
        private readonly IRoomService _roomService;
        private readonly IRoundService _roundService;
    }
}