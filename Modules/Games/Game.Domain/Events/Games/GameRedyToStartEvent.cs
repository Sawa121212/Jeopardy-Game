using Prism.Events;

namespace Game.Domain.Events.Games
{
    /// <summary>
    /// The game is ready to launch.
    /// [Игра готова к запуску]
    /// </summary>
    public class GameIsReadyToStartEvent : PubSubEvent;
}