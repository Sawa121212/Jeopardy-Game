using System;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;

namespace Common.Ui.Converters
{
    /// <summary>
    /// Получение описания перечисления
    /// </summary>
    public class EnumDescriptionConverter : MarkupExtension, IValueConverter
    {
        /// <inheritdoc />
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is Enum en ? GetDescription(en) : throw new NotSupportedException(); ;
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null && targetType.IsEnum ? Enum.ToObject(targetType, value) : throw new NotSupportedException();;
        }

        private static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo.Length > 0)
            {
                Attribute attrib = memInfo[0].GetCustomAttribute(typeof(DescriptionAttribute), false);
                if (attrib != null)
                {
                    return ((DescriptionAttribute)attrib).Description;
                }
            }
            return en.ToString();
        }
    }
}
