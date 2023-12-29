using Prism.Events;

namespace Game.Events
{
    /// <summary>
    /// The presenter in the hall has been updated.
    /// [Ведущий в комнате обновлен]
    /// </summary>
    public class HostPlayerUpdatedEvent : PubSubEvent<HostPlayerUpdatedEvent>
    {
        public HostPlayerUpdatedEvent()
        {
        }

        public HostPlayerUpdatedEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}