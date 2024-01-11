using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Common.Ui.Converters.Old
{
    /// <summary>
    /// Получение значения атрибута Description из свойства. В случае отсутствия Description возвращает имя свойства.
    /// </summary>
    public class PropertyToDescriptionConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                Type type = value.GetType();
                DescriptionAttribute descriptionAttribute = type.GetField(value.ToString()).GetCustomAttribute<DescriptionAttribute>();
                return descriptionAttribute != null ? descriptionAttribute.Description : value.ToString();
            }

            return string.Empty;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
