using System.ComponentModel;

namespace DataDomain.Rooms.Rounds.Enums
{
    /// <summary>
    /// Список раундов
    /// </summary>
    public enum RoundsLevelEnum
    {
        [Description("Первый раунд")]
        Round1 = 1,

        [Description("Второй раунд")]
        Round2 = 2,

        [Description("Третий раунд")]
        Round3 = 3,

        [Description("Перестрелка")]
        Shootout = 4,

        [Description("Финальный раунд")]
        Final = 5
    }
}