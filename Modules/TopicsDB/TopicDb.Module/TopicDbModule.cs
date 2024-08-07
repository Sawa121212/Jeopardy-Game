﻿using System.Resources;
using Common.Core.Localization;
using Prism.Ioc;
using Prism.Modularity;
using TopicDb.Module.Properties;
using TopicDb.Ui.Views;
using TopicDb.Ui.Views.Questions;
using TopicDb.Ui.Views.Topics;
using TopicsDB.Infrastructure.Interfaces.Managers;
using TopicsDB.Infrastructure.Interfaces.Services;
using TopicsDB.Infrastructure.Managers;
using TopicsDB.Infrastructure.Services;

namespace TopicDb.Module
{
    /// <summary>
    /// Модуль управления БД тем с вопросами
    /// </summary>
    public class TopicDbModule : IModule
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry

                // сперва регистрируем контекст БД с вопросам
                .RegisterSingleton<ITopicDbManager, TopicDbManager>()
                .RegisterSingleton<ITopicService, TopicService>()
                .RegisterSingleton<IQuestionService, QuestionService>();

            // регистрируем View для навигации по Регионам
            containerRegistry.RegisterForNavigation<MainTopicDbView, MainTopicDbViewModel>();
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