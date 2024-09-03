using System;
using Common.Core.Components;
using Game.Infrastructure.Interfaces.Mangers;
using Infrastructure.Interfaces.Managers;
using Prism.Ioc;
using Prism.Modularity;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;
using TelegramAPI.Infrastructure.Services.Settings;
using Users.Domain.Models;
using Users.Infrastructure.Interfaces;
using User = Telegram.Bot.Types.User;

namespace Users.Module
{
    /// <summary>
    /// Модуль управления БД пользователей
    /// </summary>
    public class UsersModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                .RegisterSingleton<IMainTelegramMenuService, MainTelegramMenuService>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            IUserService userService = containerProvider.Resolve<IUserService>();
            IAdminManager adminManager = containerProvider.Resolve<IAdminManager>();
            IGameManager gameManager = containerProvider.Resolve<IGameManager>();
            IMainTelegramMenuService mainTelegramMenuService = containerProvider.Resolve<IMainTelegramMenuService>();
            ITelegramHandlerService telegramHandlerService = containerProvider.Resolve<ITelegramHandlerService>();

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

            // Connect to room
            telegramHandlerService.RegisterHandler(StateUserEnum.MainMenu, gameManager.TryConnectPlayerToRoom, null);

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

            telegramHandlerService.RegisterHandler(StateUserEnum.InRoom,
                (u) =>
                {
                    return Result<Tuple<StateUserEnum, string>>.Fail("Вы в игровой комнате");
                },
                null);
        }
    }
}