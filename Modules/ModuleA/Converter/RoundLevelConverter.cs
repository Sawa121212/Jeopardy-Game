using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Common.Extensions;
using DataDomain.Rooms.Rounds.Enums;

namespace Game.Converter
{
    public class RoundLevelConverter : IValueConverter
    {
        /// <inheritdoc />
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            switch (value)
            {
                case null:
                    return null;
                case RoundsLevelEnum levelEnum:
                    switch (levelEnum)
                    {
                        case RoundsLevelEnum.Round1:
                        case RoundsLevelEnum.Round2:
                        case RoundsLevelEnum.Round3:
                            return levelEnum.ToInt().ToString();
                        case RoundsLevelEnum.Final:
                            return "Финальный";
                        case RoundsLevelEnum.Shootout:
                            return "Перестрелочный)";
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
            }

            return null;
        }

        /// <inheritdoc />
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}