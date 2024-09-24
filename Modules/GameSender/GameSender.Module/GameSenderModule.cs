using Common.Core.Components;
using Common.Extensions;
using Game.Infrastructure.Interfaces.Mangers;
using GameSender.Domain;
using GameSender.Infrastructure;
using GameSender.Infrastructure.Interfaces;
using Infrastructure.Interfaces.Managers;
using Prism.Ioc;
using Prism.Modularity;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using User = Telegram.Bot.Types.User;

namespace GameSender.Module
{
    /// <summary>
    /// Модуль Game
    /// </summary>
    public class GameSenderModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IGameSenderService, GameSenderService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            ITelegramHandlerService telegramHandlerService = containerProvider.Resolve<ITelegramHandlerService>();

            IGameManager gameManager = containerProvider.Resolve<IGameManager>();
            IAdminManager adminManager = containerProvider.Resolve<IAdminManager>();
            IUserService userService = containerProvider.Resolve<IUserService>();

            // Check Admin
            telegramHandlerService.RegisterHandler(StateUserEnum.CheckAddedAdmin, adminManager.CheckAddedAdminMode, null);

            //
            telegramHandlerService.RegisterHandler(StateUserEnum.SetName, userService.UpdateUsername,
                (u) =>
                {
                    Result<User>? result = telegramHandlerService.GetUser(u);

                    if (result)
                    {
                        return Result<ReplyKeyboardMarkup>.Done(
                            new ReplyKeyboardMarkup(new KeyboardButton($"{result.Value.FirstName} {result.Value.LastName}")));
                    }

                    return Result<ReplyKeyboardMarkup>.Fail(result.ErrorMessage);
                });

            // MainMenu
            telegramHandlerService.RegisterHandler(
                stateUser: StateUserEnum.MainMenu,
                handler: (update) =>
                {
                    Message message = update?.Message;

                    if (IsValidMessage(message, out Result<Tuple<StateUserEnum, string>> validateResult))
                    {
                        return validateResult;
                    }

                    User user = message.From;

                    if (user == null)
                    {
                        return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера");
                    }

                    switch (message.Text)
                    {
                        // Connect to room
                        case GameMessages.ConnectToRoom:
                            Result result = gameManager.TryConnectPlayerToRoom(user.Id);

                            if (result)
                            {
                                //Task.Run(async () => await _gameSenderService.SendConnectedPlayerActions(user.Id));
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.Player, "Вы в игровой комнате"));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail("Не удалось войти в комнату");
                            }

                            break;
                    }

                    return Result<Tuple<StateUserEnum, string>>.Fail("Я вас не понял");
                },
                (_) => Result<ReplyKeyboardMarkup>.Done(GameSenderButtons.MainButtons));

            #region InRoom

            // Player
            telegramHandlerService.RegisterHandler(
                stateUser: StateUserEnum.Player,
                handler: (update) =>
                {
                    Message message = update?.Message;

                    if (IsValidMessage(message, out Result<Tuple<StateUserEnum, string>> validateResult))
                    {
                        return validateResult;
                    }

                    User user = message.From;

                    if (user == null)
                    {
                        return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера");
                    }

                    switch (message.Text)
                    {
                        // Set to host
                        case GameMessages.SetToHost:
                            Result result = gameManager.SetPlayerToHost(user.Id);

                            if (result)
                            {
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.Host,
                                    EmojiExtension.EmojiMessageFormat(MessageEmoji.Success, "Вы стали ведущим")));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail(result.ErrorMessage);
                            }

                            break;

                        // Leave the room
                        case GameMessages.LeaveTheRoom:
                            Result leaveRoomResult = gameManager.LeaveTheRoom(user.Id);

                            if (leaveRoomResult)
                            {
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.MainMenu,
                                    EmojiExtension.EmojiMessageFormat(MessageEmoji.Success, "Вы в главном меню")));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail(leaveRoomResult.ErrorMessage);
                            }

                            break;
                        default:
                            return Result<Tuple<StateUserEnum, string>>.Fail(
                                EmojiExtension.EmojiMessageFormat(MessageEmoji.Error, "Я вас не понял"));
                    }
                },
                replyKeyboardMarkupGenerator: (_) => Result<ReplyKeyboardMarkup>.Done(GameSenderButtons.PlayerButtons));

            // Host
            telegramHandlerService.RegisterHandler(
                stateUser: StateUserEnum.Host,
                handler: (update) =>
                {
                    Message message = update?.Message;

                    if (IsValidMessage(message, out Result<Tuple<StateUserEnum, string>> validateResult))
                    {
                        return validateResult;
                    }

                    User user = message.From;

                    if (user == null)
                    {
                        return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера");
                    }

                    switch (message.Text)
                    {
                        // Set to host
                        case GameMessages.GoToPlayers:
                            Result result = gameManager.GetOutHostPlayer();

                            if (result)
                            {
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.Player,
                                    EmojiExtension.EmojiMessageFormat(MessageEmoji.Success, "Вы стали игроком")));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail(result.ErrorMessage);
                            }

                            break;

                        // Leave the room
                        case GameMessages.LeaveTheRoom:
                            Result leaveRoomResult = gameManager.LeaveTheRoom(user.Id);

                            if (leaveRoomResult)
                            {
                                //Task.Run(async () => await _gameSenderService.SendConnectedPlayerActions(user.Id));
                                return Result<Tuple<StateUserEnum, string>>.Done(
                                    new Tuple<StateUserEnum, string>(StateUserEnum.MainMenu,
                                        EmojiExtension.EmojiMessageFormat(MessageEmoji.Success, "Вы в главном меню")));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail(leaveRoomResult.ErrorMessage);
                            }

                            break;
                        default:
                            return Result<Tuple<StateUserEnum, string>>.Fail(EmojiExtension.EmojiMessageFormat(MessageEmoji.Error, "Я вас не понял"));
                    }
                },
                replyKeyboardMarkupGenerator: (_) => Result<ReplyKeyboardMarkup>.Done(GameSenderButtons.HostButtons));

            #endregion InRoom
        }

        private static bool IsValidMessage(Message? message, out Result<Tuple<StateUserEnum, string>> validationResult)
        {
            if (message == null)
            {
                validationResult = Result<Tuple<StateUserEnum, string>>.Fail("Нет сообщения");
                return true;
            }

            if (message.Type != MessageType.Text)
            {
                validationResult = Result<Tuple<StateUserEnum, string>>.Fail("тип не текстовый...");
                return true;
            }

            validationResult = null;
            return false;
        }
    }
}