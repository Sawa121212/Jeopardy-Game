using System;
using DataDomain.Data;
using DataDomain.Rooms.Rounds.Enums;
using Avalonia.Media;

namespace DataDomain.Rooms.Rounds.Helpers
{
    /// <summary>
    /// Помощник по раунду
    /// </summary>
    public static class RoundHelper
    {
        /// <summary>
        /// Получить множитель очков в раунде
        /// </summary>
        /// <param name="roundLevel"></param>
        /// <returns></returns>
        public static int GetRoundLevelMultiplier(RoundsLevelEnum roundLevel)
        {
            return roundLevel switch
            {
                RoundsLevelEnum.Round1 => 1,
                RoundsLevelEnum.Round2 => 2,
                RoundsLevelEnum.Round3 => 3,
                RoundsLevelEnum.Shootout => 1,
                RoundsLevelEnum.Final => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(roundLevel), roundLevel, null)
            };
        }

        /// <summary>
        /// Получить следующий раунд
        /// </summary>
        /// <param name="roundLevel"></param>
        /// <returns></returns>
        public static RoundsLevelEnum GetNextRoundLevel(RoundsLevelEnum? roundLevel)
        {
            roundLevel = roundLevel switch
            {
                RoundsLevelEnum.Round1 => RoundsLevelEnum.Round2,
                RoundsLevelEnum.Round2 => RoundsLevelEnum.Round3,
                RoundsLevelEnum.Round3 => RoundsLevelEnum.Final,
                RoundsLevelEnum.Final => RoundsLevelEnum.Shootout,
                RoundsLevelEnum.Shootout => RoundsLevelEnum.Shootout,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (RoundsLevelEnum) roundLevel;
        }

        /// <summary>
        /// Получить цвет комнаты для раунда
        /// </summary>
        /// <param name="roundLevel"></param>
        /// <returns></returns>
        public static SolidColorBrush GetRoundColor(RoundsLevelEnum roundLevel) => roundLevel switch
        {
            RoundsLevelEnum.Round1 => GameParameterConstants.FirstRoundColor,
            RoundsLevelEnum.Round2 => GameParameterConstants.SecondRoundColor,
            RoundsLevelEnum.Round3 => GameParameterConstants.ThirdRoundColor,
            RoundsLevelEnum.Shootout => GameParameterConstants.ThirdRoundColor, // ToDo: find color
            RoundsLevelEnum.Final => GameParameterConstants.FinalRoundColor,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
}