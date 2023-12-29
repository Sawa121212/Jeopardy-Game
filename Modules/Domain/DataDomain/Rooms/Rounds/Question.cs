namespace DataDomain.Rooms.Rounds
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public class Question
    {
        public Question(int point)
        {
            Point = point;
        }

        public string Text { get; set; }
        public object Picture { get; set; }

        public int Point { get; set; }

        /// <summary>
        /// Правильный ответ
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Вопрос был задан
        /// </summary>
        public bool IsAsked { get; set; }
    }
}