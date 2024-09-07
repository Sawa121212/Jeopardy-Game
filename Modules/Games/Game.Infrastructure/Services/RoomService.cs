using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Avalonia.Controls.Notifications;
using Common.Core.Components;
using Common.Extensions;
using DataDomain;
using DataDomain.Rooms;
using Game.Infrastructure.Interfaces.Services;
using GameSender.Infrastructure.Interfaces;
using Infrastructure.Domain.Helpers;
using Notification.Module.Services;
using ReactiveUI;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;

namespace Game.Infrastructure.Services
{
    public class RoomService : ReactiveObject, IRoomService
    {
        public RoomService(
            INotificationService notificationService,
            IUserService userService,
            IGameSenderService gameSenderService)
        {
            _notificationService = notificationService;
            _userService = userService;
            _gameSenderService = gameSenderService;
        }

        /// <summary>
        /// Комната
        /// </summary>
        private RoomModel Room
        {
            get => _room;
            set => this.RaiseAndSetIfChanged(ref _room, value);
        }

        /// <inheritdoc/>
        public bool Create()
        {
            try
            {
                Room = new RoomModel();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }

            return true;
        }

        /// <inheritdoc/>
        public bool ConnectPlayer(long playerId)
        {
            if (playerId == default)
            {
                return false;
            }

            RoomModel? room = GetRoom();

            if (room == null)
            {
                return false;
            }

            _userService.TryGetUserById(playerId, out User user);

            if (user is null)
            {
                _notificationService.Show("Error", $"User '{playerId}' not found", NotificationType.Error);

                return false;
            }

            PlayerModel player = new(user);

            room.Players.Add(player);

            return true;
        }

        /// <inheritdoc/>
        public bool AddBot()
        {
            RoomModel? room = GetRoom();

            if (room == null)
            {
                return false;
            }

            PlayerModel player = CreateBot();

            room.Players.Add(player);

            return true;
        }

        /// <inheritdoc/>
        public async Task<bool> Remove()
        {
            RoomModel? room = GetRoom();

            if (room == null)
            {
                return false;
            }

            IEnumerable<PlayerModel> players = new List<PlayerModel>(room.Players.OfType<PlayerModel>());

            foreach (PlayerModel? player in players)
            {
                await KickPlayer(player.Id).ConfigureAwait(true);
            }

            if (room.Host != null)
            {
                await KickPlayer(room.Host.Id).ConfigureAwait(true);
            }

            Room = null;

            return true;
        }

        /// <inheritdoc />
        public RoomModel? GetRoom() => Room;

        /// <inheritdoc />
        public bool SetHost(long playerId)
        {
            RoomModel? room = GetRoom();

            if (room is null)
            {
                return false;
            }

            PlayerModel? player = null;

            if (playerId != default)
            {
                player = room.Players.FirstOrDefault(e => e.Id == playerId);
            }

            if (room.Host == player)
            {
                return false;
            }

            if (room.Host is not null)
            {
                room.Players.Add(room.Host);
            }

            room.Host = player;

            if (player != null)
            {
                room.Players.Remove(player);
            }

            return true;
        }

        /// <inheritdoc />
        public Result LeaveTheRoom(long playerId)
        {
            if (playerId == default)
            {
                return Result.Fail("Неизвестный пользователь. Команда отменена!");
            }

            RoomModel? room = GetRoom();

            if (room is null)
            {
                return Result.Fail("Упс.. Комната уже закрыта");
            }

            PlayerModel? player = room.Players.FirstOrDefault(e => e != null && e.Id == playerId);

            if (player != null)
            {
                room.Players.Remove(player);
                return Result.Done();
            }

            if (room.Host != null && room.Host.Id != playerId)
            {
                return Result.Fail($"Пользователь {playerId} не найден!");
            }

            room.Host = null;
            return Result.Done();
        }

        /// <inheritdoc />
        public async Task<bool> KickPlayer(long playerId)
        {
            Result resulTask = LeaveTheRoom(playerId);

            if (!resulTask)
            {
                return false;
            }

            await _gameSenderService.SendKickedMessage(playerId);
            return true;
        }

        /// <inheritdoc />
        public GameModel? GetGame()
        {
            return GetRoom()?.Game;
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

        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;
        private readonly IGameSenderService _gameSenderService;
        private RoomModel _room;
    }
}