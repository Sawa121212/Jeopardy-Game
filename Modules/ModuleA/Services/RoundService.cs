using System;
using System.Collections.Generic;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using DataDomain.Rooms.Rounds.Helpers;
using Game.Data;
using TopicsDB.Infrastructure.Services.Interfaces;

namespace Game.Services
{
    public class RoundService : IRoundService
    {
        /// <inheritdoc />
        public List<Round> CreateGameRounds()
        {
            List<Round> rounds = new();

            RoundsLevelEnum roundLevelEnum = RoundsLevelEnum.Round1;
            bool createNextRound = true;
            while (createNextRound)
            {
                Round round = GetRound(roundLevelEnum);
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

        private Round GetRound(RoundsLevelEnum levelEnum)
        {
            int topicsCount = GameParameterConstants.TopicsCount;
            int topicLevelMultiplier = RoundHelper.GetRoundLevelMultiplier(levelEnum);
            if (levelEnum == RoundsLevelEnum.Final)
            {
                topicsCount = GameParameterConstants.FinalTopicsCount;
            }

            Round round = new(levelEnum)
            {
                Topics = new List<Topic>(topicsCount)
            };

            for (int i = 0; i < topicsCount; i++)
            {
                round.Topics.Add(CreateTopic(topicLevelMultiplier));
            }

            return round;
        }

        private Topic CreateTopic(int topicLevelMultiplier)
        {
            Topic topic = new()
            {
                Questions = CreateQuestions(topicLevelMultiplier)
            };
            return topic;
        }

        private List<Question> CreateQuestions(int topicLevelMultiplier)
        {
            List<Question> questions = new();
            while (questions.Count < GameParameterConstants.QuestionsCount)
            {
                questions.Add(new Question(GameParameterConstants.BaseQuestionPrice * (questions.Count + 1) * topicLevelMultiplier));
            }

            return questions;
        }
    }
}