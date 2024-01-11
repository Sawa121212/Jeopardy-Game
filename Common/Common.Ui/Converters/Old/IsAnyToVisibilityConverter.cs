/*
using System;
using System.Collections;
using System.Globalization;

namespace Common.Ui.Converters
{
    public class IsAnyToVisibilityConverter : OneWayMarkupConverter
    {
        /// <inheritdoc />
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICollection collection)
            {
                if (parameter is bool inverse)
                {
                    if(inverse)
                        return collection.Count > 0 ? Visibility.Collapsed : Visibility.Visible;
                }
                return collection.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }
    }
}
*/
