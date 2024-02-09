using Prism.Events;

namespace Game.Domain.Events.Questions
{
    /// <summary>
    /// Игра готова принимать ответы.
    /// </summary>
    public class GameIsReadyToReceiveAnswersEvent : PubSubEvent<GameIsReadyToReceiveAnswersEvent>
    {
        public GameIsReadyToReceiveAnswersEvent()
        {
        }

        public GameIsReadyToReceiveAnswersEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}