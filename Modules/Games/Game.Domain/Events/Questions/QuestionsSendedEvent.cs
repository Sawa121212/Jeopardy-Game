using Prism.Events;

namespace Game.Domain.Events.Questions
{
    /// <summary>
    /// Вопросы разосланы игрокам.
    /// </summary>
    public class QuestionsIsSentEvent : PubSubEvent;
}