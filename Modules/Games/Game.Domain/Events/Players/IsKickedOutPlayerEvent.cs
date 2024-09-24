using Prism.Events;

namespace Game.Domain.Events.Players
{
    /// <summary>
    /// Player kicked out.
    /// [Игрок выгнан]
    /// </summary>
    public class IsKickedOutPlayerEvent : PubSubEvent;
}