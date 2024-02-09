using Prism.Events;

namespace Game.Domain.Events.Players
{
    /// <summary>
    /// Kick a player.
    /// [Выгнать игрока]
    /// </summary>
    public class KickOutPlayerEvent : PubSubEvent<KickOutPlayerEvent>
    {
        public KickOutPlayerEvent()
        {
        }

        public KickOutPlayerEvent(string roomKey, long playerId)
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