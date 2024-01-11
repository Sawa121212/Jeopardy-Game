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
                //.RegisterSingleton<ITopicService, TopicService>()
                .RegisterSingleton<IUserService, UserService>();

            // регистрируем View для навигации по Регионам
            //containerRegistry.RegisterForNavigation<TopicListView, TopicListViewModel>();
            //containerRegistry.RegisterForNavigation<AddNewTopicView, AddNewTopicViewModel>();
            //containerRegistry.RegisterForNavigation<AddNewQuestionView, AddNewQuestionViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            //containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
        }
    }
}