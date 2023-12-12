using System;
using System.ComponentModel;
using System.Reflection;

namespace Common.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Преобразует текст к enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string enumString) where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.TryParse(enumString, true, out T value)
                ? value
                : throw new ArgumentException(nameof(enumString));
        }

        /// <summary>
        /// Преобразует текст к enum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumString"></param>
        /// <returns></returns>
        public static T ToEnumSafety<T>(this string enumString) where T : struct, IComparable, IFormattable, IConvertible
        {
            return Enum.TryParse(enumString, true, out T value)
                ? value
                : default(T);
        }
        
        /// <summary>
        /// Преобразует enum к int
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="soure"></param>
        /// <returns></returns>
        public static int ToInt<T>(this T soure) where T :struct, IComparable, IFormattable, IConvertible
        {
            if (!typeof(T).IsEnum)
                throw new ArgumentException($"{typeof(T)} must be an enumerated type");

            return (int)(IConvertible)soure;
        }

        /// <summary>
        /// Достать описание из атрибутов перечислимого типа.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            FieldInfo? fieldInfo = value.GetType().GetField(value.ToString());
            if (fieldInfo == null) return null;
            DescriptionAttribute? attribute = (DescriptionAttribute)fieldInfo.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute != null ? attribute.Description : value.ToString();
        }

        /// <summary>
        /// Содержит ли перечисление хотя бы один из указанных флагов.
        /// </summary>
        /// <param name="flags">Flags enumeration to check</param>
        /// <param name="testFlags">Flag to check for</param>
        /// <returns></returns>
        public static bool ContainsAny(this Enum flags, Enum testFlags)
        {
            if (flags == null)
                throw new ArgumentNullException(nameof(flags));

            if (testFlags == null)
                throw new ArgumentNullException(nameof(testFlags));

            if (flags.GetType() != testFlags.GetType())
            {
                throw new ArgumentException($"Enumeration type mismatch. The flag is of type '{testFlags.GetType()}', was expecting '{flags.GetType()}'.");
            }

            Type? underlyingType = Enum.GetUnderlyingType(flags.GetType());
            dynamic flagsType = Convert.ChangeType(flags, underlyingType);
            dynamic testFlagsType = Convert.ChangeType(testFlags, underlyingType);
            return (flagsType & testFlagsType) != 0;
        }

        /// <summary>
        /// Содержит ли перечисление только указанные флаги.
        /// </summary>
        /// <param name="flags">Flags enumeration to check</param>
        /// <param name="testFlags">Flag to check for</param>
        /// <returns></returns>
        public static bool ContainsExact(this Enum flags, Enum testFlags)
        {
            if (flags == null)
                throw new ArgumentNullException(nameof(flags));

            if (testFlags == null)
                throw new ArgumentNullException(nameof(testFlags));

            if (flags.GetType() != testFlags.GetType())
            {
                throw new ArgumentException($"Enumeration type mismatch. The flag is of type '{testFlags.GetType()}', was expecting '{flags.GetType()}'.");
            }
            Type? underlyingType = Enum.GetUnderlyingType(flags.GetType());
            dynamic flagsType = Convert.ChangeType(flags, underlyingType);
            dynamic testFlagsType = Convert.ChangeType(testFlags, underlyingType);
            return (flagsType & testFlagsType) == testFlagsType;
        }

        /// <summary>
        /// Установка флагов.
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="addingFlags"></param>
        /// <returns></returns>
        public static T Add<T>(this Enum flags, T addingFlags) where T : struct, IComparable, IFormattable, IConvertible
        {
            Type? underlyingType = Enum.GetUnderlyingType(flags.GetType());

            dynamic flagsType = Convert.ChangeType(flags, underlyingType);
            dynamic addingFlagsType = Convert.ChangeType(addingFlags, underlyingType);

            flagsType |= addingFlagsType;

            return (T)flagsType;
        }

        /// <summary>
        /// Очистка флагов.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="flags"></param>
        /// <param name="removingFlags"></param>
        /// <returns></returns>
        public static T Remove<T>(this Enum flags, T removingFlags) where T : struct, IComparable, IFormattable, IConvertible
        {
            Type? underlyingType = Enum.GetUnderlyingType(flags.GetType());

            dynamic flagsType = Convert.ChangeType(flags, underlyingType);
            dynamic removingFlagsType = Convert.ChangeType(removingFlags, underlyingType);
            flagsType &= ~removingFlagsType;

            return (T)flagsType;
        }

        /// <summary>
        /// Перебор флагов для какого то действия.
        /// </summary>
        /// <param name="flags"></param>
        /// <param name="processFlag"></param>
        public static void ForEach<T>(this Enum flags, Action<T> processFlag) where T : struct, IComparable, IFormattable, IConvertible
        {
            if (processFlag == null)
                throw new ArgumentNullException(nameof(processFlag));

            Type? underlyingType = Enum.GetUnderlyingType(flags.GetType());
            dynamic flagsAsInt = Convert.ChangeType(flags, underlyingType);

            for (int bit = 1; bit != 0; bit <<= 1)
            {
                dynamic? temp = flagsAsInt & bit;
                if (temp != 0)
                    processFlag((T)temp);
            }
        }
    }
}
