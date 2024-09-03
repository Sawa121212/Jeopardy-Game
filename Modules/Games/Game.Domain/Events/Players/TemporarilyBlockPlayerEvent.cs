using Prism.Events;

namespace Game.Domain.Events.Players
{
    /// <summary>
    /// Временно заблокировать игрока, не принимать ответы
    /// </summary>
    public class TemporarilyBlockPlayerEvent : PubSubEvent<long>;
}