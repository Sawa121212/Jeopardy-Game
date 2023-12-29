using System.ComponentModel;

namespace DataDomain.Rooms.Rounds.Enums
{
    /// <summary>
    /// Список раундов
    /// </summary>
    public enum RoundsLevelEnum
    {
        [Description("Перестрелка")]
        Shootout = 1,
        
        [Description("Финальный раунд")]
        Final = 1,

        [Description("Первый раунд")]
        Round1 = 1,

        [Description("Второй раунд")]
        Round2 = 2,

        [Description("Третий раунд")]
        Round3 = 3,
    }
}