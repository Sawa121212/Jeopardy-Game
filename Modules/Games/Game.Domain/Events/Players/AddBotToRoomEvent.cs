using Prism.Events;

namespace Game.Domain.Events.Players
{
    /// <summary>
    /// Add Bot to the room.
    /// [Добавить бота в комнату]
    /// </summary>
    public class AddBotToRoomEvent : PubSubEvent<string>
    {
    }
}