using System.Resources;
using Common.Core.Localization;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TopicDb.Domain;
using TopicDb.Module.Properties;
using TopicsDB.Infrastructure;
using TopicsDB.Infrastructure.Interfaces;

namespace TopicDb.Module
{
    /// <summary>
    /// Модуль A
    /// </summary>
    public class TopicDbModule : IModule
    {
        private readonly IRegionManager _regionManager;
        private readonly TopicDbContext _topicDbContext;

        public TopicDbModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
            _topicDbContext = new TopicDbContext();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry
                // сперва регистрируем контекст БД с вопросам
                .RegisterInstance(_topicDbContext)
                .RegisterSingleton<ITopicService, TopicService>()
                .RegisterSingleton<IQuestionService, QuestionService>();

            // регистрируем View для навигации по Регионам
            //containerRegistry.RegisterForNavigation<TabAView>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));

            // Зарегистрировать View к региону. Теперь при запуске ПО View будет показано
            //_regionManager.RegisterViewWithRegion("RegionA", typeof(TabAView));
        }
    }
}