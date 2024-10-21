namespace Users.Domain.Models
{
    public enum StateUserEnum : uint
    {
        //Main
        SetName,
        MainMenu,
        CheckAddedAdmin,

        // Room
        InRoom,
        Host,

        // Game
        Playing,
        IsReadyReceiveAnswer
    }
}