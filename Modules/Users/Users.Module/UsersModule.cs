using System.Resources;
using Common.Core.Components;
using Common.Core.Localization;
using Prism.Ioc;
using Prism.Modularity;
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

            containerProvider.Resolve<ITelegramHandlerService>().RegisterHandler(StateUserEnum.SetName, userService.UpdateUsername);
            containerProvider.Resolve<ITelegramHandlerService>().RegisterHandler(StateUserEnum.MainMenu,
                (m) => { return Result<StateUserEnum>.Fail("Вы в главном меню, но пока тут пусто"); });
        }
    }
}