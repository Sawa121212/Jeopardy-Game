using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Common.Extensions;

namespace Common.Ui.Converters
{
    public class MathOperationConverter : MarkupExtension, IValueConverter
    {
        private object MathFunc(string operation, double x, Type targetType, bool inversia = false)
        {
            double c = 0.0;
            if (!operation.IsNullOrEmpty())
            {
                if (double.TryParse(operation.Substring(1).Trim(), out double y))
                {
                    switch (operation[0])
                    {
                        case '+':
                            c = inversia ? x - y : x + y;
                            break;
                        case '-':
                            c = inversia ? x + y : x - y;
                            break;
                        case '*':
                            c = inversia ? x / y : x * y;
                            break;
                        case '/':
                            c = inversia ? x * y : x / y;
                            break;
                    }
                    try
                    {
                        return System.Convert.ChangeType(c, targetType);
                    }
                    catch { }
                }
            }
            return null;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string operation)
            {
                {
                    if (value is double x)
                    {
                        return MathFunc(operation, x, targetType) ?? throw new NotSupportedException();;
                    }
                }
                {
                    if (value is int x)
                    {
                        return MathFunc(operation, x, targetType) ?? throw new NotSupportedException();;
                    }
                }
            }
            throw new NotSupportedException();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string operation)
            {
                {
                    if (value is double x)
                    {
                        return MathFunc(operation, x, targetType, true) ?? throw new NotSupportedException();;
                    }
                }
                {
                    if (value is int x)
                    {
                        return MathFunc(operation, x, targetType, true) ?? throw new NotSupportedException();;
                    }
                }
            }
            throw new NotSupportedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}