using Prism.Events;

namespace Game.Domain.Events.Rooms
{
    /// <summary>
    /// The presenter in the hall has been updated.
    /// [Ведущий в комнате обновлен]
    /// </summary>
    public class HostPlayerUpdatedEvent : PubSubEvent<string>
    {
    }
}