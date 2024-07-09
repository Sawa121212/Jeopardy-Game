using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Notifications;
using Common.Extensions;
using DataDomain.Data;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Infrastructure.Interfaces.Services;
using Infrastructure.Domain.Helpers;
using Notification.Module.Services;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Infrastructure.Services
{
    public partial class RoundService : IRoundService
    {
        public RoundService(ITopicService topicService, IQuestionService questionService, INotificationService notificationService)
        {
            _topicService = topicService;
            _questionService = questionService;
            _notificationService = notificationService;
        }

        /// <inheritdoc />
        public List<RoundModel?>? CreateGameRounds()
        {
            List<RoundModel?>? rounds = CollectRounds();
            if (rounds is null || !rounds.Any())
            {
                _notificationService.Show("Error", "No rounds created.", NotificationType.Error);
                return null;
            }

            // Выставить специальные вопросы
            AddSpecialQuestions(rounds);

            return rounds;
        }

        /// <summary>
        /// Собрать раунд
        /// </summary>
        /// <returns></returns>
        private List<RoundModel?>? CollectRounds()
        {
            List<RoundModel?>? generatedRounds = new();

            // выбранные темы
            List<TopicModel> selectedTopics = new();

            // topics
            List<Topic> allTopics = _topicService.GetAllTopics();

            // количество тем в игре
            int gameTopicsCount = GameParameterConstants.BaseRoundTopicsCount * 3
                                  + GameParameterConstants.FinalRoundTopicsCount
                                  + GameParameterConstants.ShootoutRoundTopicsCount;

            // Test. Uncomment.
            // Проверяем, есть ли вообще нужное количество тем для генерации игры
            /*if (topicsCount < gameTopicsCount)
            {
                _notificationService.Show("Error", "The required number of topics was not found.", NotificationType.Error);
                return null;
            }*/

            RoundsLevelEnum levelEnum = RoundsLevelEnum.Round1;

            for (int i = 0; i < levelEnum.NamesLength(); i++)
            {
                int topicsMaxCount = default;
                switch (levelEnum)
                {
                    case RoundsLevelEnum.Round1:
                    case RoundsLevelEnum.Round2:
                    case RoundsLevelEnum.Round3:
                        topicsMaxCount = GameParameterConstants.BaseRoundTopicsCount;
                        break;
                    case RoundsLevelEnum.Shootout:
                        topicsMaxCount = GameParameterConstants.ShootoutRoundTopicsCount;
                        break;
                    case RoundsLevelEnum.Final:
                        topicsMaxCount = GameParameterConstants.FinalRoundTopicsCount;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(levelEnum), levelEnum, null);
                }

                // Получить множитель очков в раунде
                int topicLevelMultiplier = RoundHelper.GetRoundLevelMultiplier(levelEnum);

                RoundModel? round = new(levelEnum)
                {
                    Topics = new List<TopicModel>(topicsMaxCount)
                };

                // выбираем темы для раунда
                for (int index = 0; index < topicsMaxCount; index++)
                {
                    if (!allTopics.Any())
                    {
                        return null;
                    }

                    Topic? topic = GetRandomFullTopic(allTopics);

                    if (topic is null)
                    {
                        break;
                    }

                    // Если вдруг тему мы уже используем. (Возможно надо удалить!)
                    /*while (selectedTopics.FirstOrDefault(t => t.Id == topic.Id) != null)
                    {
                        // тема уже используется
                        allTopics.Remove(topic);
                        topic = GetRandomFullTopic(allTopics);
                    }*/

                    TopicModel? collectedTopic = CreateTopicModel(topic, topicLevelMultiplier);
                    if (collectedTopic is null)
                    {
                        return null;
                    }

                    round.Topics.Add(collectedTopic);
                    selectedTopics.Add(collectedTopic);
                }

                if (round.Topics.Count != topicsMaxCount)
                {
                    return null;
                }

                generatedRounds.Add(round);
                levelEnum = RoundHelper.GetNextRoundLevel(levelEnum);
            }

            return generatedRounds;
        }

        /// <summary>
        /// Собрать тему
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="topicLevelMultiplier"></param>
        /// <returns></returns>
        private TopicModel? CreateTopicModel(Topic? topic, int topicLevelMultiplier)
        {
            if (topic == null)
            {
                return null;
            }

            TopicModel? topicModel = new()
            {
                Id = topic.Id,
                Name = topic.Name,
                Questions = CreateQuestionModels(topicLevelMultiplier, topic.Id)
            };
            return topicModel;
        }

        /// <summary>
        /// Собрать модель вопросов
        /// </summary>
        /// <param name="topicLevelMultiplier"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        private List<QuestionModel> CreateQuestionModels(int topicLevelMultiplier, int topicId)
        {
            List<QuestionModel> questions = new();

            while (questions.Count < GameParameterConstants.TopicQuestionsCount)
            {
                int questionBasePrice = GetQuestionBasePrice(questions.Count + 1);
                Question? question = _questionService.GetRandomQuestionFromTopicByPrice(topicId, questionBasePrice);
                questions.Add(new QuestionModel(question, questionBasePrice * topicLevelMultiplier));
            }

            return questions;
        }

        /// <summary>
        /// Получить случайную полную тему
        /// </summary>
        /// <param name="topics"></param>
        /// <returns></returns>
        private Topic? GetRandomFullTopic(IList<Topic> topics)
        {
            if (!topics.Any())
            {
                return null;
            }

            Topic? randomFullTopic = null;

            // получаем рандомную полную тему
            while (true)
            {
                if (!topics.Any())
                {
                    // если закончились вопросы
                    return null;
                }

                int index = RandomGenerator.GetRandom().Next(0, topics.Count);
                randomFullTopic = topics[index];

                List<Question> allQuestionsByTopic = _questionService.GetAllQuestionsByTopicId(randomFullTopic.Id);

                // для начала будем считать, что тема полная
                bool isFullQuestion = true;

                for (int questionIndex = 0; questionIndex < GameParameterConstants.TopicQuestionsCount; questionIndex++)
                {
                    bool questionExists = allQuestionsByTopic
                        .Any(question =>
                            question.TopicId == randomFullTopic.Id
                            && question.Price == GetQuestionBasePrice(questionIndex + 1));

                    if (questionExists)
                    {
                        // Есть вопрос с указанной ценой. Ищем дальше
                        continue;
                    }

                    // вопрос с указанной ценой отсутствует
                    // значит тема не полная, удаляем из списка
                    isFullQuestion = false;
                    topics.Remove(randomFullTopic);
                    break;
                }

                if (!isFullQuestion)
                {
                    continue;
                }

                // Тема полная, удаляем из списка
                // Test. Uncomment
                //topics.Remove(randomFullTopic);
                break;
            }

            return randomFullTopic;
        }

        /// <summary>
        /// Получить базовую цену вопроса по его номеру
        /// </summary>
        /// <param name="questionsIndex"></param>
        /// <returns></returns>
        private int GetQuestionBasePrice(int questionsIndex) => GameParameterConstants.BaseQuestionPrice * questionsIndex;

        private readonly ITopicService _topicService;
        private readonly IQuestionService _questionService;
        private readonly INotificationService _notificationService;
    }
}