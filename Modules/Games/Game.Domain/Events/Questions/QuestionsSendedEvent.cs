using Prism.Events;

namespace Game.Domain.Events.Questions
{
    /// <summary>
    /// Вопросы разосланы игрокам.
    /// </summary>
    public class QuestionsIsSentEvent : PubSubEvent<QuestionsIsSentEvent>
    {
        public QuestionsIsSentEvent()
        {
        }

        public QuestionsIsSentEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}