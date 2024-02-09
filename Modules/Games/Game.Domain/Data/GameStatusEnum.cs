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
    }
}