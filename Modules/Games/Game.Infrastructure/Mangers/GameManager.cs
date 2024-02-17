using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Common.Extensions;
using DataDomain;
using DataDomain.Rooms;
using Game.Domain.Events.Games;
using Game.Domain.Events.Players;
using Game.Domain.Events.Rooms;
using Game.Infrastructure.Interfaces.Mangers;
using Game.Infrastructure.Interfaces.Services;
using Notification.Module.Services;
using Prism.Events;
using ReactiveUI;

namespace Game.Infrastructure.Mangers
{
    public class GameManager : ReactiveObject, IGameManager
    {
        public GameManager(
            IEventAggregator eventAggregator,
            INotificationService notificationService,
            IRoomService roomService,
            IRoundService roundService)
        {
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _roomService = roomService;
            _roundService = roundService;

            _eventAggregator.GetEvent<PlayerIsTryingToConnectToRoomEvent>().Subscribe(e => ConnectPlayerToRoom(e.RoomKey, e.PlayerId));
            _eventAggregator.GetEvent<SetPlayerToHostEvent>().Subscribe(e => SetPlayerToHost(e.RoomKey, e.PlayerId));
            _eventAggregator.GetEvent<KickOutPlayerEvent>().Subscribe(async (e) => await KickOutPlayer(e.RoomKey, e.PlayerId));
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

                if (room.Game.Rounds != null && room.Game.Rounds.Any())
                {
                    room.Game.IsStarted = true;
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
        public IEnumerable<PlayerModel?> GetPlayersFromRoom(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return null;
            }

            return _roomService.GetRoomByKey(roomKey)?.Players;
        }

        /// <inheritdoc />
        public PlayerModel? GetHostPlayerFromRoom(string roomKey)
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
        public async Task<bool> CloseRoom(string roomKey)
        {
            if (roomKey.IsNullOrEmpty())
            {
                return false;
            }

            return await _roomService.Remove(roomKey).ConfigureAwait(true);
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
        private async Task KickOutPlayer(string roomKey, long playerId)
        {
            if (await _roomService.KickPlayer(roomKey, playerId))
            {
                _eventAggregator.GetEvent<PlayerKickedOutEvent>().Publish(new PlayerKickedOutEvent(roomKey, playerId));
            }
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;
        private readonly IRoomService _roomService;
        private readonly IRoundService _roundService;
    }
}