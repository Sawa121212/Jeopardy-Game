/*using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Инвертировать значение Boolean для отображения
    /// </summary>
    
    public class InverseBooleanToVisibilityConverter : MarkupExtension, IValueConverter
    {
        /// <summary>Converts a value.</summary>
        /// <param name="value">The value produced by the binding source.</param>
        /// <param name="targetType">The type of the binding target property.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool flag && targetType == typeof(Visibility))
            {
                if (parameter is string paramString && paramString.Equals("inverse", StringComparison.InvariantCultureIgnoreCase))
                {
                    flag = !flag;
                }

                if ((bool?)parameter == true)
                    flag = !flag;

                return flag ? Visibility.Collapsed : Visibility.Visible;
            }

            throw new ArgumentException("Invalid argument/return type. Expected argument: bool and return type: Visibility");
        }

        /// <summary>Converts a value.</summary>
        /// <param name="value">The value that is produced by the binding target.</param>
        /// <param name="targetType">The type to convert to.</param>
        /// <param name="parameter">The converter parameter to use.</param>
        /// <param name="culture">The culture to use in the converter.</param>
        /// <returns>
        /// A converted value. If the method returns null, the valid null value is used.
        /// </returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility && targetType == typeof(bool))
            {
                var flag = visibility == Visibility.Collapsed;
                if (parameter is string paramString && paramString.Equals("inverse", StringComparison.InvariantCultureIgnoreCase))
                {
                    flag = !flag;
                }

                if ((bool?)parameter == true)
                {
                    flag = !flag;
                }

                return flag;
            }

            throw new ArgumentException("Invalid argument/return type. Expected argument: Visibility and return type: bool");
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}*/