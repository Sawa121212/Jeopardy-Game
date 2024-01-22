using Common.Core.Components;
using Prism.Ioc;
using Prism.Modularity;
using Users.Infrastructure;

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
                //.RegisterSingleton<ITopicDbManager, TopicDbManager>()
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
            IUserService userService = containerProvider.Resolve<IUserService>();

            containerProvider.Resolve<ITelegramHandlerService>().RegisterHandler(Domain.StateUserEnum.SetName, userService.UpdateUsername);
            containerProvider.Resolve<ITelegramHandlerService>().RegisterHandler(Domain.StateUserEnum.MainMenu,
                (m) => 
                {
                    return Result<Domain.StateUserEnum>.Fail("Вы в главном меню, но пока тут пусто");
                });
        }
    }
}