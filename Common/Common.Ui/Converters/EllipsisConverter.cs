using System;
using System.Globalization;
using System.IO;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Common.Extensions;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Укорачивание имени файла и т.п.
    /// </summary>
    public class EllipsisConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            int maxLength = 100;
            string stringParam = parameter?.ToString();
            if (stringParam != null)
            {
                int.TryParse(stringParam, out maxLength);
            }
            string small = value?.ToString().EllipsisString(maxLength, Path.DirectorySeparatorChar);
            return small;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
