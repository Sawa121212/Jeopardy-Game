using System;
using System.Globalization;
using Common.Extensions.Properties;

namespace Common.Extensions
{
    /// <summary>
    /// Методы расширения числовых значений.
    /// </summary>
    public static class NumericExtensions
    {
        /// <inheritdoc cref="Bound(int,int,int)"/>
        public static double Bound(this double value, double min, double max)
        {
            CommonPhrases.Culture = CultureInfo.CurrentUICulture; // устанавливаем яз. стандарт для фраз

            if (double.IsNaN(value) || double.IsNaN(min) || double.IsNaN(max))
                throw new InvalidOperationException();

            if (min > max)
                throw new ArgumentException(CommonPhrases.Exception_ParamRangeIsInvalid);

            if (value > max) return max;
            if (value < min) return min;

            return value;
        }

        /// <summary>
        /// Замена значения <see cref="double.NaN"/> на указанное.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <param name="defaultValue">Значение, на которое следует заменить <see cref="double.NaN"/>.</param>
        /// <returns>Полученное значение.</returns>
        public static double NaNToDouble(this double value, double defaultValue)
        {
            return double.IsNaN(value) ? defaultValue : value;
        }
    }
}