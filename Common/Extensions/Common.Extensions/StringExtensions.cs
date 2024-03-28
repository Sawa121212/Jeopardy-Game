using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using Common.Extensions.Properties;

namespace Common.Extensions
{
    /// <summary>
    /// Методы-расширения для <see cref="string" />.
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class StringExtensions
    {
        /// <summary>
        /// Проверка строки на null и пустоту.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результат проверки.</returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// Проверка строки на пустоту.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результат проверки.</returns>
        public static bool IsEmpty(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return text.Length == 0;
        }

        /// <summary>
        /// Признак того, что строка содержит только пробельные (white space) символы.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результат проверки.</returns>
        public static bool IsWhiteSpace(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return text.All(char.IsWhiteSpace);
        }
        
        /// <summary>
        /// Признак того, что строка есть null, или пустая, или содержит только пробельные (white space) символы.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результирующая строка.</returns>
        public static bool IsNullOrWhiteSpace(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }
        
        /// <summary>
        /// Если длина строки больше <paramref name="maxLength"/> символов, метод
        /// возвращает только первые <paramref name="maxLength"/> символов строки, иначе - исходную строку.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="maxLength">Максимальная допустимая длина строки.</param>
        /// <returns>Усечённая в случае превышения максимальной длины строка.</returns>
        public static string Truncate(this string text, int maxLength)
        {
            CommonPhrases.Culture = CultureInfo.CurrentUICulture;       // устанавливаем яз. стандарт для фраз
            
            if (maxLength < 0)
                throw new ArgumentOutOfRangeException(nameof(maxLength), 
                    CommonPhrases.Exception_ParamIsNegative);

            return text.IsEmpty() ? text : text.Substring(0, Math.Min(text.Length, maxLength));
        }

        /// <summary>
        /// Укорачивание строки до заданной длины с учетом delimiter.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="maxLength">Максимальная длина строки.</param>
        /// <param name="delimiter">Разделитель, до которого укорачивается строка (если их несколько, - то до последнего;
        /// например DirectorySeparatorChar.</param>
        /// <returns>Укороченная строка.</returns>
        public static string EllipsisString(this string text, int maxLength, char delimiter)
        {
            if (text.Length <= maxLength)
                return text;
            List<string> parts = text.Split(delimiter).ToList();
            if (parts.Count > 1)
            {
                StringBuilder builder = new();
                string lastPart = parts.Last();
                int max = maxLength - lastPart.Length - 3;
                for (int i = 0; i < parts.Count - 1; i++)
                {
                    if (builder.Length < max)
                    {
                        builder.AppendFormat($"{parts[i]}{delimiter}");
                    }
                }

                builder.AppendFormat($"...{delimiter}{lastPart}");
                return builder.ToString();
            }

            return text.Truncate(maxLength);
        }
        
        /// <summary>
        /// Получение форматированной строки, путем замены элементов формата в строке format
        /// соответствующими элементами массива args. 
        /// </summary>
        public static string Format(this string format, params object[] args)
        {
            if (format == null || args == null)
                throw new ArgumentNullException(format == null ? nameof(format) : nameof(args));

            return string.Format(format, args);
        }

        /// <summary>
        /// Выполняет делегат <see cref="Func{T, TResult}"/> над исходной строкой,
        /// при условии установленного признака <paramref name="isExecute"/>.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="isExecute">Признак необходимости выполнения делегата.</param>
        /// <param name="func">Делегат.</param>
        /// <returns>Результирующая строка.</returns>
        public static string Func(this string text, bool isExecute, Func<string, string> func = null)
        {
            return (func == null || !isExecute)
                ? text
                : func(text);
        }
    }
}