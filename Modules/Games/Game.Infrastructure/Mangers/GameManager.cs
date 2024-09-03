using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Common.Core.Components;
using Common.Extensions;
using DataDomain;
using DataDomain.Rooms;
using Game.Domain.Events.Games;
using Game.Domain.Events.Players;
using Game.Domain.Events.Players.Host;
using Game.Domain.Events.Rooms;
using Game.Infrastructure.Interfaces.Mangers;
using Game.Infrastructure.Interfaces.Services;
using GameSender.Domain;
using GameSender.Infrastructure.Interfaces;
using Notification.Module.Services;
using Prism.Events;
using ReactiveUI;
using Telegram.Bot.Types;
using Users.Domain.Models;

namespace Game.Infrastructure.Mangers
{
    public partial class GameManager : ReactiveObject, IGameManager
    {
        public GameManager(
            IEventAggregator eventAggregator,
            INotificationService notificationService,
            IRoomService roomService,
            IRoundService roundService,
            IGameSenderService gameSenderService)
        {
            _eventAggregator = eventAggregator;
            _notificationService = notificationService;
            _roomService = roomService;
            _roundService = roundService;
            _gameSenderService = gameSenderService;

            _eventAggregator.GetEvent<AddBotToRoomEvent>().Subscribe(AddBot);

            _eventAggregator.GetEvent<SetPlayerToHostEvent>().Subscribe(SetPlayerToHost);
            _eventAggregator.GetEvent<GetOutHostPlayerEvent>().Subscribe(GetOutHostPlayer);
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

                _eventAggregator.GetEvent<GameIsStartedEvent>().Publish();
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
        public GameModel? GetGame() => _roomService.GetGame();

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
        public Result<Tuple<StateUserEnum, string>> TryConnectPlayerToRoom(Update update)
        {
            Message message = update?.Message;

            if (message == null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет сообщения");
            }

            if (message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("тип не текстовый...");
            }

            Telegram.Bot.Types.User user = message.From;

            if (user == null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера");
            }

            if (message.Text != GameMessages.ConnectToRoom)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("тип не текстовый...");
            }

            RoomModel? room = _roomService.GetRoom();

            if (room is null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Упс.. Комната уже закрыта");
            }

            if (room.Players.Any(e => e != null && e.Id == user.Id))
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Вы уже находитесь в комнате");
            }

            if (_roomService.ConnectPlayer(user.Id))
            {
                _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Publish();
                Task.Run(async () => await _gameSenderService.SendConnectedPlayerActions(user.Id));
                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.InRoom, "Вы в игровой комнате"));
            }
            else
            {
                _notificationService.Show("Ошибка", $"Игрок {user.Username} не смог присоединится в комнату", NotificationType.Error);
                return Result<Tuple<StateUserEnum, string>>.Fail("Не удалось войти в комнату");
            }
        }

        /// <summary>
        /// Добавить бота
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        private void AddBot()
        {
            _roomService.AddBot();
            _eventAggregator.GetEvent<NumberOfPlayersInRoomIsUpdatedEvent>().Publish();
        }

        /// <summary>
        /// Установить игрока в качестве организатора
        /// </summary>
        /// <param name=""></param>
        /// <param name="playerId"></param>
        private void SetPlayerToHost(long playerId)
        {
            if (_roomService.SetHost(playerId))
            {
                _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Publish();
            }
        }

        private void GetOutHostPlayer()
        {
            if (_roomService.SetHost(default))
            {
                _eventAggregator.GetEvent<HostPlayerUpdatedEvent>().Publish();
            }
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name=""></param>
        /// <param name="playerId"></param>
        private async Task KickOutPlayer(long playerId)
        {
            if (await _roomService.KickPlayer(playerId))
            {
                _eventAggregator.GetEvent<PlayerKickedOutEvent>().Publish(playerId);
            }
        }

        private readonly IEventAggregator _eventAggregator;
        private readonly INotificationService _notificationService;
        private readonly IRoomService _roomService;
        private readonly IRoundService _roundService;
        private readonly IGameSenderService _gameSenderService;
    }
}