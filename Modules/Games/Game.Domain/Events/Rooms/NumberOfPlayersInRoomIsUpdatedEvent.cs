using Prism.Events;

namespace Game.Domain.Events.Rooms
{
    /// <summary>
    /// The number of players in the room has been updated.
    /// [Количество игроков комнате обновилось]
    /// </summary>
    public class NumberOfPlayersInRoomIsUpdatedEvent : PubSubEvent;
}