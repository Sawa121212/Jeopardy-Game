namespace DataDomain.Rooms.Rounds
{
    /// <summary>
    /// Вопрос
    /// </summary>
    public class Question
    {
        public string Text { get; }
        public object Picture { get; }

        public int Point { get; }

        /// <summary>
        /// Правильный ответ
        /// </summary>
        public string Answer { get; }

        /// <summary>
        /// Вопрос был задан
        /// </summary>
        public bool IsTheQuestionWasAsked { get; set; }
    }
}