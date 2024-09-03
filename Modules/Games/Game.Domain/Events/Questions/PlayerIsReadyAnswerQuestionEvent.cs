using Prism.Events;

namespace Game.Domain.Events.Questions
{
    /// <summary>
    /// Игрок готов ответить на вопрос (Нажал на кнопку).
    /// </summary>
    public class PlayerIsReadyAnswerQuestionEvent : PubSubEvent<long>;
}