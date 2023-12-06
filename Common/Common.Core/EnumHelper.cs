using System;
using System.ComponentModel;
using System.Reflection;

namespace Common.Core
{
    public static class EnumHelper
    {
        public static string GetDescription<T>(this T enumValue)
            where T : struct, IConvertible
        {
            if (!typeof(T).IsEnum)
                return null;

            string? description = enumValue.ToString();
            FieldInfo? fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

            if (fieldInfo != null)
            {
                object[]? attrs = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), true);
                if (attrs != null && attrs.Length > 0)
                {
                    description = ((DescriptionAttribute) attrs[0]).Description;
                }
            }

            return description;
        }
    }
}