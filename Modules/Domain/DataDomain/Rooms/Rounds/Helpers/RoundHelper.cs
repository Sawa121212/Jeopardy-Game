using System;
using DataDomain.Rooms.Rounds.Enums;

namespace DataDomain.Rooms.Rounds.Helpers
{
    public static class RoundHelper
    {
        public static int GetRoundLevelMultiplier(RoundsLevelEnum levelEnum)
        {
            return levelEnum switch
            {
                RoundsLevelEnum.Round1 => 1,
                RoundsLevelEnum.Round2 => 2,
                RoundsLevelEnum.Round3 => 3,
                RoundsLevelEnum.Shootout => 1,
                RoundsLevelEnum.Final => 1,
                _ => throw new ArgumentOutOfRangeException(nameof(levelEnum), levelEnum, null)
            };
        }

        public static RoundsLevelEnum GetNextRoundLevel(RoundsLevelEnum? roundLevelEnum)
        {
            roundLevelEnum = roundLevelEnum switch
            {
                RoundsLevelEnum.Round1 => RoundsLevelEnum.Round2,
                RoundsLevelEnum.Round2 => RoundsLevelEnum.Round3,
                RoundsLevelEnum.Round3 => RoundsLevelEnum.Shootout,
                RoundsLevelEnum.Shootout => RoundsLevelEnum.Final,
                RoundsLevelEnum.Final => null,
                _ => throw new ArgumentOutOfRangeException()
            };

            return (RoundsLevelEnum) roundLevelEnum;
        }
    }
}