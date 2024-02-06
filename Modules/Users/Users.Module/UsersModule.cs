using System;
using System.Resources;
using Common.Core.Components;
using Common.Core.Localization;
using Prism.Ioc;
using Prism.Modularity;
using TelegramAPI.Test.Services.Settings;
using Users.Domain.Models;
using Users.Infrastructure;
using Users.Infrastructure.Interfaces;
using Users.Infrastructure.Interfaces.Managers;
using Users.Infrastructure.Managers;

namespace Users.Module
{
    /// <summary>
    /// Модуль A
    /// </summary>
    public class UsersModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                // сперва регистрируем контекст БД с вопросам
                .RegisterSingleton<IUserDbManager, UserDbManager>()
                .RegisterSingleton<IUserService, UserService>()
                .RegisterSingleton<IMainTelegramMenuService, MainTelegramMenuService>()
                .RegisterSingleton<ITelegramHandlerService, TelegramHandlerService>();

            // регистрируем View для навигации по Регионам
            //containerRegistry.RegisterForNavigation<TopicListView, TopicListViewModel>();
            //containerRegistry.RegisterForNavigation<AddNewTopicView, AddNewTopicViewModel>();
            //containerRegistry.RegisterForNavigation<AddNewQuestionView, AddNewQuestionViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            IUserService userService = containerProvider.Resolve<IUserService>();
            IMainTelegramMenuService mainTelegramMenuService = containerProvider.Resolve<IMainTelegramMenuService>();

            ITelegramHandlerService telegramHandlerService = containerProvider.Resolve<ITelegramHandlerService>();
            telegramHandlerService.RegisterHandler(StateUserEnum.SetName,
                userService.UpdateUsername,
                (u) => 
                {
                    var result = telegramHandlerService.GetUser(u);
                    if (result)
                        return Result<Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup>.Done(new Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup(new Telegram.Bot.Types.ReplyMarkups.KeyboardButton($"{result.Value.FirstName} {result.Value.LastName}")));
                    return Result<Telegram.Bot.Types.ReplyMarkups.ReplyKeyboardMarkup>.Fail(result.ErrorMessage);
                });

            telegramHandlerService.RegisterHandler(StateUserEnum.MainMenu,
                (u) => { if (u.Message.Text == "Войти в комнату")
                        return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.InRoom, "Вы в игровой комнате"));
                    return Result<Tuple<StateUserEnum, string>>.Fail("Вы в главном меню, и я не понимаю, куда тебе нужно..."); },
                mainTelegramMenuService.CreateMenu);

            telegramHandlerService.RegisterHandler(StateUserEnum.InRoom,
                (u) => { return Result<Tuple<StateUserEnum, string>>.Fail("Вы в игровой комнате"); },
                null);
        }
    }
}