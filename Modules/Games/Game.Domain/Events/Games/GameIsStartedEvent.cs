using Prism.Events;

namespace Game.Domain.Events.Games
{
    /// <summary>
    /// The game started.
    /// [Игра началась]
    /// </summary>
    public class GameIsStartedEvent : PubSubEvent<GameIsStartedEvent>
    {
        public GameIsStartedEvent()
        {
        }

        public GameIsStartedEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}