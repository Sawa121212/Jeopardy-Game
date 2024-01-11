using System;
using System.Collections;
using System.Globalization;
using Avalonia.Data.Converters;

namespace Common.Ui.Converters.Old
{
    /// <summary>
    /// Возвращает true если в IList есть хоть один элемент.
    /// </summary>
    public class IsAnyToBooleanConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
            {
                if (parameter is bool inverse)
                {
                    if (inverse)
                        return !(collection.Count > 0);
                }

                return collection.Count > 0;
            }

            return false;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}