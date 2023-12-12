using System;
using System.Globalization;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Конвертер заглушкой на ConvertBack.
    /// </summary>
    public abstract class OneWayMarkupConverter : MarkupConverter
    {
        /// <inheritdoc />
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}