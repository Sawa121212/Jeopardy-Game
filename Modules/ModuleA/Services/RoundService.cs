using System.Collections.Generic;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
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

            while (true)
            {
                Round round = GetRound(roundLevelEnum);
                if (!rounds.Contains(round))
                {
                    rounds.Add(round);

                    switch (roundLevelEnum)
                    {
                        case RoundsLevelEnum.Round1:
                            roundLevelEnum = RoundsLevelEnum.Round2;
                            break;
                        case RoundsLevelEnum.Round2:
                            roundLevelEnum = RoundsLevelEnum.Round3;
                            break;
                        case RoundsLevelEnum.Round3:
                            roundLevelEnum = RoundsLevelEnum.Final;
                            break;
                        default:
                        {
                            if (roundLevelEnum == RoundsLevelEnum.Final)
                                roundLevelEnum = RoundsLevelEnum.Shootout;
                            break;
                        }
                    }
                }

                if (roundLevelEnum == RoundsLevelEnum.Shootout)
                {
                    break;
                }
            }


            return rounds;
        }

        private Round GetRound(RoundsLevelEnum levelEnum)
        {
            int topicsCount = GameParameterConstants.TopicsCount;
            int topicLevel = (int) levelEnum;
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
                round.Topics.Add(CreateTopic(topicLevel));
            }

            return round;
        }

        private Topic CreateTopic(int topicLevel)
        {
            Topic topic = new()
            {
                Questions = CreateQuestions(topicLevel)
            };
            return topic;
        }

        private List<Question> CreateQuestions(int topicLevel)
        {
            List<Question> questions = new();
            while (questions.Count < GameParameterConstants.QuestionsCount)
            {
                questions.Add(new Question(GameParameterConstants.BaseQuestionPrice * (questions.Count + 1) * topicLevel));
            }

            return questions;
        }
    }
}