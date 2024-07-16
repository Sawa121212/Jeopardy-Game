using Prism.Events;

namespace Game.Domain.Events.Players.Host
{
    /// <summary>
    /// Set a player as event organizer.
    /// [Установить игрока в качестве организатора]
    /// </summary>
    public class SetPlayerToHostEvent : PubSubEvent<SetPlayerToHostEvent>
    {
        public SetPlayerToHostEvent()
        {
        }

        public SetPlayerToHostEvent(string roomKey, long playerId)
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