using System;
using System.Globalization;
using Common.Extensions;
using Common.Ui.Converters;
using Confirmation.Domain.Enums;

namespace Confirmation.Ui.Converters
{
    public class ConfirmationToVisibilityConverter : MarkupConverter
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ConfirmationResultEnum result)
            {
                if (parameter is string mode)
                {
                    ConfirmationResultEnum flag = mode.ToEnum<ConfirmationResultEnum>();
                    return result.ContainsAny(flag) ? targetType.IsVisible : !targetType.IsVisible;
                }
            }

            return !targetType.IsVisible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new Exception();
        }
    }
}