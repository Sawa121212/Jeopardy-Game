using Common.Core.Components;
using Game.Infrastructure.Interfaces.Mangers;
using GameSender.Domain;
using GameSender.Infrastructure;
using GameSender.Infrastructure.Interfaces;
using Infrastructure.Interfaces.Managers;
using Prism.Ioc;
using Prism.Modularity;
using Telegram.Bot.Types;
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
                        return Result<Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup>.Done(
                            new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(
                                new Telegram.Bot.Types.ReplyMarkups.KeyboardButton($"{result.Value.FirstName} {result.Value.LastName}")));

                    return Result<Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup>.Fail(result.ErrorMessage);
                });

            // MainMenu (ConnectToRoom)
            telegramHandlerService.RegisterHandler(StateUserEnum.MainMenu,
                (update) =>
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
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.InRoom, "Вы в игровой комнате"));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail("Не удалось войти в комнату");
                            }

                            break;
                    }

                    return Result<Tuple<StateUserEnum, string>>.Fail("Я вас не понял");
                }, null);

            // InRoom
            telegramHandlerService.RegisterHandler(StateUserEnum.InRoom,
                (update) =>
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
                                //Task.Run(async () => await _gameSenderService.SendConnectedPlayerActions(user.Id));
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.InRoom, "✔ Вы в игровой комнате"));
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
                                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.InRoom, "\u2705 Вы в игровой комнате"));
                            }
                            else
                            {
                                return Result<Tuple<StateUserEnum, string>>.Fail(leaveRoomResult.ErrorMessage);
                            }

                            break;
                        default:
                            return Result<Tuple<StateUserEnum, string>>.Fail("\u274c Я вас не понял");
                    }
                }, null);

            /*telegramHandlerService.RegisterHandler(StateUserEnum.MainMenu,
                (u) =>
                {
                    if (u.Message.Text == "Войти в комнату")
                    {
                        return gameManager.TryConnectPlayerToRoom(string roomKey, long playerId);

                        // event
                        return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.InRoom, "Вы в игровой комнате"));
                    }

                    return Result<Tuple<StateUserEnum, string>>.Fail("Вы в главном меню, и я не понимаю, куда тебе нужно...");
                },
                mainTelegramMenuService.CreateMenu);*/

            /*telegramHandlerService.RegisterHandler(StateUserEnum.InRoom,
                (u) =>
                {
                    return Result<Tuple<StateUserEnum, string>>.Fail("Вы в игровой комнате");
                },
                null);*/
        }
    }
}