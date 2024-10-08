namespace Users.Domain.Models
{
    public enum StateUserEnum : uint
    {
        //Main
        SetName,
        MainMenu,
        CheckAddedAdmin,

        // Room
        Player,
        Host,

        // Game
    }
}