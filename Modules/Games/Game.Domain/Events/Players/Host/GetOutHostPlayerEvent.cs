using Prism.Events;

namespace Game.Domain.Events.Players.Host
{
    /// <summary>
    /// Get out host player as event organizer.
    /// [Убрать игрока в качестве организатора]
    /// </summary>
    public class GetOutHostPlayerEvent : PubSubEvent<GetOutHostPlayerEvent>
    {
        public GetOutHostPlayerEvent()
        {
        }

        public GetOutHostPlayerEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}