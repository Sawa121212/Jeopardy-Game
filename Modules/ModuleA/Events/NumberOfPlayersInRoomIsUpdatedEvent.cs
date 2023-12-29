using Prism.Events;

namespace Game.Events
{
    /// <summary>
    /// The number of players in the room has been updated.
    /// [Количество игроков комнате обновилось]
    /// </summary>
    public class NumberOfPlayersInRoomIsUpdatedEvent : PubSubEvent<NumberOfPlayersInRoomIsUpdatedEvent>
    {
        public NumberOfPlayersInRoomIsUpdatedEvent()
        {
        }

        public NumberOfPlayersInRoomIsUpdatedEvent(string roomKey)
        {
            RoomKey = roomKey;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }
    }
}