using System.ComponentModel;

namespace Game.Domain.Data
{
    public enum GameStatusEnum
    {
        [Description("Продолжить")]
        Continue,

        [Description("Показать все темы предстоящей игры")]
        ShowAllTopics,

        [Description("Показать название раунда")]
        ShowRoundLevel,

        [Description("Показать текущий раунд")]
        ShowCurrentRound,

        [Description("Перейти к следущему раунду")]
        GoNextRound,

        [Description("Учесть ставки игроков")]
        SetPlayerBets,

        [Description("Конец игры. Показать победителя")]
        EndGame_ShowWinner
    }
}