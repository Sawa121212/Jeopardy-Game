using System;
using System.Collections.Generic;
using DataDomain.Data;
using DataDomain.Rooms.Rounds;
using DataDomain.Rooms.Rounds.Enums;
using Infrastructure.Domain.Helpers;

namespace Game.Infrastructure.Services
{
    public partial class RoundService
    {
        /// <summary>
        /// Выставить специальные вопросы
        /// </summary>
        /// <param name="rounds"></param>
        private void AddSpecialQuestions(List<RoundModel?> rounds)
        {
            IEnumerable<SpecialQuestionEnum> specialQuestions = new[]
            {
                SpecialQuestionEnum.CatInPoke, SpecialQuestionEnum.QuestionAuction
            };

            foreach (RoundModel? round in rounds)
            {
                if (round?.Topics == null)
                {
                    continue;
                }

                int specialQuestionsCount = 0;

                switch (round.Level)
                {
                    case RoundsLevelEnum.Round1:
                    case RoundsLevelEnum.Round3:
                        specialQuestionsCount = GameParameterConstants.SpecialQuestionsCountOnBaseRound;
                        break;
                    case RoundsLevelEnum.Round2:
                        specialQuestionsCount = GameParameterConstants.SpecialQuestionsCountOnSecondRound;
                        break;
                    case RoundsLevelEnum.Final:
                    case RoundsLevelEnum.Shootout:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (specialQuestionsCount == 0)
                {
                    continue;
                }

                // Выставить специальные вопросы по очереди
                foreach (SpecialQuestionEnum specialQuestion in specialQuestions)
                {
                    for (int i = 0; i < specialQuestionsCount; i++)
                    {
                        while (true)
                        {
                            int randomTopicIndex = RandomGenerator.GetRandom().Next(0, round.Topics.Count);
                            int randomQuestion = RandomGenerator.GetRandom().Next(0, round.Topics[randomTopicIndex].Questions.Count);

                            QuestionModel questionModel = round.Topics[randomTopicIndex].Questions[randomQuestion];
                            if (questionModel.SpecialQuestion != SpecialQuestionEnum.None)
                            {
                                continue;
                            }

                            questionModel.SpecialQuestion = specialQuestion;
                            break;
                        }
                    }
                }
            }
        }
    }
}