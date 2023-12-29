using Prism.Events;

namespace Game.Events
{
    /// <summary>
    /// The player is trying to connect to the room.
    /// [Игрок пытается подключиться в комнату]
    /// </summary>
    public class PlayerIsTryingToConnectToRoomEvent : PubSubEvent<PlayerIsTryingToConnectToRoomEvent>
    {
        public PlayerIsTryingToConnectToRoomEvent()
        {
        }

        public PlayerIsTryingToConnectToRoomEvent(string roomKey, long playerId)
        {
            RoomKey = roomKey;
            PlayerId = playerId;
        }

        /// <summary>
        /// Room key
        /// </summary>
        public string RoomKey { get; }

        /// <summary>
        /// User id
        /// </summary>
        public long PlayerId { get; }
    }
}