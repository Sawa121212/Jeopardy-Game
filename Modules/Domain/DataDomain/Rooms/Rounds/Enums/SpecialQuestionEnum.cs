using System.ComponentModel;

namespace DataDomain.Rooms.Rounds.Enums
{
    /// <summary>
    /// Специальные вопросы
    /// </summary>
    public enum SpecialQuestionEnum
    {
        [Description("Не специальные вопросы")]
        None = 0,

        [Description("Кот в мешке")]
        CatInPoke = 1,

        [Description("Вопрос-аукцион")]
        QuestionAuction = 2,
    }
}