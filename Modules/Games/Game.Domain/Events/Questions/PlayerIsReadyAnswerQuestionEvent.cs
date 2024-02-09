using Prism.Events;

namespace Game.Domain.Events.Questions
{
    /// <summary>
    /// Игрок готов ответить на вопрос (Нажал на кнопку).
    /// </summary>
    public class PlayerIsReadyAnswerQuestionEvent : PubSubEvent<PlayerIsReadyAnswerQuestionEvent>
    {
        public PlayerIsReadyAnswerQuestionEvent()
        {
        }

        public PlayerIsReadyAnswerQuestionEvent(string roomKey, long playerId)
        {
            RoomKey = roomKey;
            PlayerId = playerId;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }

        /// <summary>
        /// User id
        /// </summary>
        public long PlayerId { get; }
    }
}