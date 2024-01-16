﻿using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Controls.Notifications;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Data;
using Infrastructure.Domain.Helpers;
using Notification.Module.Services;
using TopicDb.Domain.Models;
using TopicsDB.Infrastructure.Interfaces.Services;

namespace Game.Services
{
    public class RoundService : IRoundService
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
            List<RoundModel?>? rounds = new();

            RoundsLevelEnum roundLevelEnum = RoundsLevelEnum.Round1;
            bool createNextRound = true;
            while (createNextRound)
            {
                RoundModel? round = CollectRound(roundLevelEnum);
                if (round is null)
                {
                    _notificationService.Show("Error", "No rounds created.", NotificationType.Error);
                    return null;
                }

                if (!rounds.Contains(round))
                {
                    rounds.Add(round);

                    switch (roundLevelEnum)
                    {
                        case RoundsLevelEnum.Round1:
                        case RoundsLevelEnum.Round2:
                        case RoundsLevelEnum.Round3:
                        case RoundsLevelEnum.Shootout:
                            roundLevelEnum = RoundHelper.GetNextRoundLevel(roundLevelEnum);
                            break;
                        case RoundsLevelEnum.Final:
                            createNextRound = false;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }

            return rounds;
        }

        /// <summary>
        /// Собрать раунд
        /// </summary>
        /// <param name="levelEnum"></param>
        /// <returns></returns>
        private RoundModel? CollectRound(RoundsLevelEnum levelEnum)
        {
            // Проверяем, есть ли вообще нужное количество тем
            int topicsCount = _topicService.GetAllTopicsCount();
            if (topicsCount < GameParameterConstants.TopicsCount)
            {
                _notificationService.Show("Error", "The required number of topics was not found.", NotificationType.Error);
                return null;
            }

            int topicsMaxCount = GameParameterConstants.TopicsCount;
            int topicLevelMultiplier = RoundHelper.GetRoundLevelMultiplier(levelEnum);
            if (levelEnum == RoundsLevelEnum.Final)
            {
                topicsMaxCount = GameParameterConstants.FinalTopicsCount;
            }

            RoundModel? round = new(levelEnum)
            {
                Topics = new List<TopicModel>(topicsMaxCount)
            };

            // Collect topics
            for (int index = 0; index < topicsMaxCount; index++)
            {
                Topic? topic = GetRandomFullTopic();

                if (topic is null)
                {
                    break;
                }

                while (round.Topics.FirstOrDefault(t => t.Id == topic.Id) == null)
                {
                    topic = GetRandomFullTopic();
                }

                TopicModel collectTopic = CollectTopic(topic, topicLevelMultiplier);
                round.Topics.Add(collectTopic);
            }

            return round.Topics.Count == topicsMaxCount ? round : null;
        }

        /// <summary>
        /// Собрать тему
        /// </summary>
        /// <param name="topic"></param>
        /// <param name="topicLevelMultiplier"></param>
        /// <returns></returns>
        private TopicModel CollectTopic(Topic? topic, int topicLevelMultiplier)
        {
            TopicModel topicModel = new()
            {
                Questions = CollectQuestions(topicLevelMultiplier, topic.Id)
            };
            return topicModel;
        }

        /// <summary>
        /// Собрать вопросы
        /// </summary>
        /// <param name="topicLevelMultiplier"></param>
        /// <param name="topicId"></param>
        /// <returns></returns>
        private List<QuestionModel> CollectQuestions(int topicLevelMultiplier, int topicId)
        {
            List<QuestionModel> questions = new();
            int questionBasePrice = GetQuestionBasePrice(questions.Count + 1);
            while (questions.Count < GameParameterConstants.QuestionsCount)
            {
                Question? question = _questionService.GetRandomQuestionFromTopicByPrice(topicId, questionBasePrice);
                questions.Add(new QuestionModel(question, questionBasePrice * topicLevelMultiplier));
            }

            return questions;
        }


        private Topic? GetRandomFullTopic()
        {
            int topicsCount = _topicService.GetAllTopicsCount();
            Topic? randomFullTopic = null;

            // получаем рандомную полную тему
            while (true)
            {
                int index = RandomGenerator.GetRandom().Next(0, topicsCount);
                randomFullTopic = _topicService.GetTopicById(index);

                List<Question> allQuestionsByTopic = _questionService.GetAllQuestionsByTopicId(randomFullTopic.Id);

                // тема полная
                bool isFullQuestion = true;

                for (int questionIndex = 0; questionIndex < GameParameterConstants.QuestionsCount; questionIndex++)
                {
                    bool questionExists = allQuestionsByTopic
                        .Any(question => question.TopicId == randomFullTopic.Id && question.Price == GetQuestionBasePrice(questionIndex));
                    if (questionExists)
                    {
                        continue;
                    }

                    isFullQuestion = false;
                    break;
                }

                if (isFullQuestion)
                {
                    break;
                }
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