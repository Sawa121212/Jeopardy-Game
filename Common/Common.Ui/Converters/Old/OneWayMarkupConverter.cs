using System;
using System.Globalization;

namespace Common.Ui.Converters.Old
{
    /// <summary>
    /// Конвертер заглушкой на ConvertBack.
    /// </summary>
    public abstract class OneWayMarkupConverter : MarkupConverter
    {
        /// <inheritdoc />
        public override object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}