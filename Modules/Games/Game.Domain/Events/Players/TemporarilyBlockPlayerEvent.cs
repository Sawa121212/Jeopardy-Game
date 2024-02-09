using Prism.Events;

namespace Game.Domain.Events.Players
{
    /// <summary>
    /// Временно заблокировать игрока, не принимать ответы
    /// </summary>
    public class TemporarilyBlockPlayerEvent : PubSubEvent<TemporarilyBlockPlayerEvent>
    {
        public TemporarilyBlockPlayerEvent()
        {
        }

        public TemporarilyBlockPlayerEvent(long playerId)
        {
            PlayerId = playerId;
        }

        /// <summary>
        /// User id
        /// </summary>
        public long PlayerId { get; }
    }
}