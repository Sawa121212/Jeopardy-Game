using System.Collections.Generic;
using Common.Extensions;
using DataDomain.Rooms.Rounds.Enums;

namespace DataDomain.Rooms.Rounds
{
    /// <summary>
    /// Раунд
    /// </summary>
    public class Round
    {
        public Round(RoundsLevelEnum level)
        {
            Level = level;
        }

        /// <summary>
        /// Номер раунда
        /// </summary>
        public RoundsLevelEnum Level { get; }

        /// <summary>
        /// Имя раунда. 1, 2, 3 или Финальный раунд
        /// </summary>
        public string Name => Level.GetDescription();

        /// <summary>
        /// Тема
        /// </summary>
        public string Theme { get; }

        /// <summary>
        /// Вопросы
        /// </summary>
        public List<Question> Questions { get; } // max 5

        /// <summary>
        /// Получить баллы за ответ на вопрос
        /// </summary>
        /// <param name="question">Вопрос</param>
        /// <param name="isRightAnswer">Это правильный ответ</param>
        /// <returns></returns>
        public int GetQuestionPoint(Question question, bool isRightAnswer)
        {
            int quality = isRightAnswer ? 1 : -1;
            if (Questions.Contains(question))
            {
                return question.Point * (int) Level * quality;
            }

            return 0;
        }
    }
}