using Avalonia.Media;

namespace DataDomain.Data
{
    public static class GameParameterConstants
    {
        /// <summary>
        /// Базовая стоимость вопроса
        /// </summary>
        public static int BaseQuestionPrice = 100;

        /// <summary>
        /// Количество тем
        /// </summary>
        public static int BaseRoundTopicsCount = 6;

        public static int ShootoutRoundTopicsCount = 2;

        /// <summary>
        /// Количество тем финального раунда
        /// </summary>
        public static int FinalRoundTopicsCount = 7;

        /// <summary>
        /// Количество вопросов
        /// </summary>
        public static int TopicQuestionsCount = 5;

        /// <summary>
        /// В ходе первого раунда студия подсвечивается синим цветом
        /// </summary>
        public static SolidColorBrush FirstRoundColor = new(Colors.Blue);

        /// <summary>
        /// В ходе первого раунда студия подсвечивается золотисто-коричневым
        /// </summary>
        public static SolidColorBrush SecondRoundColor = new(Colors.DarkGoldenrod);

        /// <summary>
        /// В ходе первого раунда студия подсвечивается золотисто-коричневым
        /// </summary>
        public static SolidColorBrush ThirdRoundColor = new(Colors.Green);

        /// <summary>
        /// В ходе первого раунда студия подсвечивается золотисто-коричневым
        /// </summary>
        public static SolidColorBrush FinalRoundColor = new(Colors.Purple);

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