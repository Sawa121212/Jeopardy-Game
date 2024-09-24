using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Common.Core.Components;
using DataDomain;
using DataDomain.Rooms;
using Game.Domain.Events.Games;
using Game.Domain.Events.Players;
using Game.Domain.Events.Rooms;
using Game.Infrastructure.Interfaces.Mangers;
using Game.Infrastructure.Interfaces.Services;
using Infrastructure.Domain.Helpers;
using Notification.Module.Services;
using Prism.Events;
using ReactiveUI;

namespace Game.Infrastructure.Mangers
{
    public partial class GameManager : ReactiveObject, IGameManager
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

            _eventAggregator.GetEvent<AddBotToRoomEvent>().Subscribe(() => AddBot());

            _eventAggregator.GetEvent<KickOutPlayerEvent>().Subscribe(async (e) => await KickOutPlayer(e));
            _eventAggregator.GetEvent<GameIsReadyToStartEvent>().Subscribe(StartGame);
        }

        /// <inheritdoc />
        public bool CreateRoom()
        {
            return _roomService.Create();
        }

        private void StartGame()
        {
            RoomModel? room = _roomService.GetRoom();

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

                _eventAggregator.GetEvent<IsStartedGameEvent>().Publish();
            }
            else
            {
                // error
            }
        }

        /// <inheritdoc />
        public IEnumerable<PlayerModel?> GetPlayersFromRoom() => _roomService.GetRoom()?.Players;

        /// <inheritdoc />
        public PlayerModel? GetHostPlayerFromRoom() => _roomService.GetRoom()?.Host;

        /// <inheritdoc />
        public GameModel? GetGame() => _roomService.GetRoom()?.Game;

        /// <inheritdoc />
        public bool CloseGame()
        {
            RoomModel? room = _roomService.GetRoom();

            if (room == null)
            {
                return false;
            }

            room.Game = null;

            return true;
        }

        /// <inheritdoc />
        public async Task<bool> CloseRoom()
        {
            return await _roomService.Remove().ConfigureAwait(true);
        }

        /// <inheritdoc />
        public Result LeaveTheRoom(long userId)
        {
            Result result = _roomService.LeaveTheRoom(userId);

            if (result)
            {
                _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Publish();
            }

            return result;
        }

        /// <summary>
        /// Присоединить игрока к комнате
        /// </summary>
        /// <param name="playerId"></param>
        public Result TryConnectPlayerToRoom(long playerId)
        {
            if (playerId == default)
            {
                return Result.Fail("Неизвестный пользователь. Команда отменена!");
            }

            RoomModel? room = _roomService.GetRoom();

            if (room is null)
            {
                return Result.Fail("Упс.. Комната уже закрыта");
            }

            if (room.Players.Any(e => e != null && e.Id == playerId))
            {
                return Result.Fail("Вы уже находитесь в комнате");
            }

            if (_roomService.ConnectPlayer(playerId))
            {
                _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Publish();
            }
            else
            {
                _notificationService.Show("Ошибка", $"Игрок {playerId} не смог присоединится в комнату", NotificationType.Error);
                return Result.Fail($"В комнату игрок не смог присоединится");
            }

            return Result.Done();
        }

        /// <summary>
        /// Установить игрока в качестве организатора
        /// </summary>
        /// <param name="playerId"></param>
        public Result SetPlayerToHost(long playerId)
        {
            if (playerId == default)
            {
                return Result.Fail("Неизвестный пользователь. Команда отменена!");
            }

            RoomModel? room = _roomService.GetRoom();

            if (room is null)
            {
                return Result.Fail("Упс.. Комната уже закрыта");
            }

            if (room.Host != null && room.Host.Id == playerId)
            {
                return Result.Fail("Вы уже ведущий");
            }

            if (_roomService.SetHost(playerId))
            {
                _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Publish();
                return Result.Done();
            }
            else
            {
                // ToDo: error
                return Result.Fail("Set Host Error");
            }
        }

        public Result GetOutHostPlayer()
        {
            RoomModel? room = _roomService.GetRoom();

            if (room is null)
            {
                return Result.Fail("Упс.. Комната уже закрыта");
            }

            if (_roomService.SetHost(default))
            {
                _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Publish();
                return Result.Done();
            }

            return Result.Fail("Get Out Host Player Error");
        }

        /// <inheritdoc/>
        public bool AddBot()
        {
            Result<RoomModel> result = _roomService.TryGetRoom();

            if (!result)
            {
                return Result.Fail(result.ErrorMessage);
            }

            RoomModel room = result.Value;

            PlayerModel player = CreateBot();
            room.Players.Add(player);

            _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Publish();
            return true;
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="playerId"></param>
        private async Task KickOutPlayer(long playerId)
        {
            if (await _roomService.KickPlayer(playerId))
            {
                _eventAggregator.GetEvent<IsKickedOutPlayerEvent>().Publish();
            }
        }

        /// <summary>
        /// Создать бота
        /// </summary>
        private PlayerModel CreateBot()
        {
            int id = RandomGenerator.GenerateSixDigitRandomNumber();

            return new PlayerModel
            {
                Id = id,
                Name = $"Bot {id}"
            };
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;
        private readonly IRoomService _roomService;
        private readonly IRoundService _roundService;
    }
}