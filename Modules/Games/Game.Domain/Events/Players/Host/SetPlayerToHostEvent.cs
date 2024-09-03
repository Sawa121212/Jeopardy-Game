using Prism.Events;

namespace Game.Domain.Events.Players.Host
{
    /// <summary>
    /// Set a player as event organizer.
    /// [Установить игрока в качестве организатора]
    /// </summary>
    public class SetPlayerToHostEvent : PubSubEvent<long>;
}