using System.Drawing;

namespace Game.Data
{
    internal static class GameParameterConstants
    {
        /// <summary>
        /// Базовая стоимость вопроса
        /// </summary>
        public static int BaseQuestionPrice = 100;

        /// <summary>
        /// Количество раундов
        /// </summary>
        public static int RoundsCount = 4;

        /// <summary>
        /// Количество тем
        /// </summary>
        public static int TopicsCount = 6;

        /// <summary>
        /// Количество тем финального раунда
        /// </summary>
        public static int FinalTopicsCount = 7;

        /// <summary>
        /// Количество вопросов
        /// </summary>
        public static int QuestionsCount = 5;

        /// <summary>
        /// В ходе первого раунда студия подсвечивается синим цветом
        /// </summary>
        public static Color FirstRoundColor = Color.Blue;

        /// <summary>
        /// В ходе первого раунда студия подсвечивается золотисто-коричневым
        /// </summary>
        public static Color SecondRoundColor = Color.DarkGoldenrod;

        /// <summary>
        /// В ходе первого раунда студия подсвечивается золотисто-коричневым
        /// </summary>
        public static Color ThirdRoundColor = Color.Green;

        /// <summary>
        /// В ходе первого раунда студия подсвечивается золотисто-коричневым
        /// </summary>
        public static Color FinalRoundColor = Color.Purple;
    }
}