using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common.Core.Components;
using DataDomain;
using DataDomain.Rooms;
using GameSender.Infrastructure.Interfaces;
using ReactiveUI;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;

namespace Game.Infrastructure.Services
{
    internal class RoomService : ReactiveObject
    {
        public RoomService(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Комната
        /// </summary>
        private RoomModel Room
        {
            get => _room;
            set => this.RaiseAndSetIfChanged(ref _room, value);
        }

        /// <summary>
        /// Создать комнату
        /// </summary>
        /// <returns></returns>
        public Result Create()
        {
            try
            {
                Room = new RoomModel();
            }
            catch (Exception e)
            {
                return Result.Fail("Не удалось создать комнату");
            }

            return Result.Done();
        }

        /// <summary>
        /// Удалить комнату
        /// </summary>
        /// <returns></returns>
        public async Task<Result> Remove()
        {
            Result<RoomModel> result = TryGetRoom();

            if (!result)
            {
                return Result.Fail(result.ErrorMessage);
            }

            RoomModel? room = result.Value;

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

            return Result.Done();
        }

        /// <summary>
        /// Получить комнату
        /// </summary>
        /// <returns></returns>
        public RoomModel? GetRoom() => Room;

        /// <summary>
        /// Получить комнату
        /// </summary>
        /// <returns></returns>
        public Result<RoomModel> TryGetRoom()
        {
            RoomModel? room = GetRoom();

            return room is null
                ? Result<RoomModel>.Fail("Не удалось найти созданную комнату")
                : Result<RoomModel>.Done(room);
        }

        /// <summary>
        /// Присоединить игрока к комнате
        /// </summary>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        public Result ConnectPlayer(long playerId)
        {
            if (playerId == default)
            {
                return Result.Fail("Неизвестный пользователь. Команда отменена!");
            }

            Result<RoomModel> result = TryGetRoom();

            if (!result)
            {
                return Result.Fail(result.ErrorMessage);
            }

            _userService.TryGetUserById(playerId, out User user);

            if (user is null)
            {
                string message = $"Пользователь '{playerId}' не определен";
                return Result.Fail(message);
            }

            PlayerModel player = new(user);

            result.Value.Players.Add(player);

            return Result.Done();
        }

        /// <summary>
        /// Установить игрока "Ведущим"
        /// </summary>
        /// <param name="playerId">ИД игрока</param>
        /// <returns></returns>
        public Result SetHost(long playerId)
        {
            Result<RoomModel> result = TryGetRoom();

            if (!result)
            {
                return Result.Fail(result.ErrorMessage);
            }

            RoomModel room = result.Value;

            PlayerModel? player = null;

            if (playerId != default)
            {
                player = room.Players.FirstOrDefault(e => e.Id == playerId);
            }

            if (room.Host == player)
            {
                return Result.Fail($"Пользователь '{playerId}' уже является ведущим");
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

            return Result.Done();
        }

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Выгнать игрока
        /// </summary>
        /// <param name="playerId"></param>
        /// <returns></returns>
        public async Task<Result> KickPlayer(long playerId)
        {
            Result resulTask = LeaveTheRoom(playerId);

            if (!resulTask)
            {
                return Result.Fail(resulTask.ErrorMessage);
            }

            await _gameSenderService.SendKickedMessage(playerId);
            return Result.Done();
        }

        private readonly IUserService _userService;
        private readonly IGameSenderService _gameSenderService;
        private RoomModel _room;
    }
}