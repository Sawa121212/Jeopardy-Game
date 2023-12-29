using Prism.Events;

namespace Game.Events
{
    /// <summary>
    /// Player kicked out.
    /// [Игрок выгнан]
    /// </summary>
    public class PlayerKickedOutEvent : PubSubEvent<PlayerKickedOutEvent>
    {
        public PlayerKickedOutEvent()
        {
        }

        public PlayerKickedOutEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}