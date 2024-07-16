using Prism.Events;

namespace Game.Domain.Events.Players.Host
{
    /// <summary>
    /// Get out host player as event organizer.
    /// [Убрать игрока в качестве организатора]
    /// </summary>
    public class GetOutHostPlayerEvent : PubSubEvent<string>
    {
    }
}