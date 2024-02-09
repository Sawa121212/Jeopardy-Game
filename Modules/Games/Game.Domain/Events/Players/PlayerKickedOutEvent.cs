using Prism.Events;

namespace Game.Domain.Events.Players
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

        public PlayerKickedOutEvent(string roomKey, long playerId)
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