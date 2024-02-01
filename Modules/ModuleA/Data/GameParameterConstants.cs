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
        /// Количество тем
        /// </summary>
        public static int TopicsCount = 6;

        public static int ShootoutTopicsCount = 2;

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

        /// <summary>
        /// Количество специальных вопросов в 1 и 3 раундах
        /// </summary>
        public static int SpecialQuestionsCountOnBaseRound = 2;

        /// <summary>
        /// Количество специальных вопросов в 2 раунде
        /// </summary>
        public static int SpecialQuestionsCountOnSecondRound = 1;
    }
}