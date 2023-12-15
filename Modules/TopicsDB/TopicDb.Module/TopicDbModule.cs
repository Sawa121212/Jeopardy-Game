﻿using System.Resources;
using Common.Core.Localization;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TopicDb.Domain;
using TopicDb.Module.Properties;
using TopicsDB.Infrastructure.Services;
using TopicsDB.Infrastructure.Services.Interfaces;
using TopicsDB.Infrastructure.Views;
using TopicsDB.Infrastructure.Views.Questions;
using TopicsDB.Infrastructure.Views.Topics;

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
            containerRegistry.RegisterForNavigation<TopicListView, TopicListViewModel>();
            containerRegistry.RegisterForNavigation<AddNewTopicView, AddNewTopicViewModel>();
            containerRegistry.RegisterForNavigation<AddNewQuestionView, AddNewQuestionViewModel>();
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // Добавим ресурс Локализации в "коллекцию ресурсов локализации"
            containerProvider.Resolve<ILocalizer>().AddResourceManager(new ResourceManager(typeof(Language)));
        }
    }
}