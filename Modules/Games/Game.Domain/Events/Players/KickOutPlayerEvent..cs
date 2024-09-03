using Prism.Events;

namespace Game.Domain.Events.Players
{
    /// <summary>
    /// Kick a player.
    /// [Выгнать игрока]
    /// </summary>
    public class KickOutPlayerEvent : PubSubEvent<long>
    {
    }
}