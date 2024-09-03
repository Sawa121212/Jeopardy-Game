using Prism.Events;

namespace Game.Domain.Events.Questions
{
    /// <summary>
    /// Игра готова принимать ответы.
    /// </summary>
    public class GameIsReadyToReceiveAnswersEvent : PubSubEvent;
}