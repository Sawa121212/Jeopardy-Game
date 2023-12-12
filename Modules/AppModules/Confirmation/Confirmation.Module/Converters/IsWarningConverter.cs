using System;
using System.Globalization;
using Avalonia.Data;
using Common.Ui.Converters;
using Confirmation.Module.Models;

namespace Confirmation.Module.Converters
{
    public class IsWarningConverter : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Warning;
        }

        /// <inheritdoc />
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return BindingOperations.DoNothing;
        }
    }
}