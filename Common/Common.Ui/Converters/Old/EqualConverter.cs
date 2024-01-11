using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Common.Ui.Converters.Old
{
    /// <summary>
    /// Сравнивает значения value и parameter
    /// </summary>
    /// <example> IsEnabled="{Binding ReadOnlyMode, Converter={StaticResource EqualConverter}, 
    /// ConverterParameter={markup:Boolean False}, Mode=OneWay, UpdateSourceTrigger=PropertyChanged}"></example>
    public class EqualConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter) ?? parameter == null;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value?.Equals(parameter) ?? parameter == null;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
