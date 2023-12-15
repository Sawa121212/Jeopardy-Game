﻿using System;
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
        /// Проверка строки на null.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результат проверки.</returns>
        public static bool IsNull(this string text)
        {
            return text is null;
        }
        
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
        /// Замена пустой строки на null.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результирующая строка.</returns>
        public static string EmptyToNull(this string text)
        {
            return text != null && text.IsEmpty() ? null : text;
        }

        /// <summary>
        /// Замена null строки на пустую.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результирующая строка.</returns>
        public static string NullToEmpty(this string text)
        {
            return text ?? string.Empty;
        }
        
        /// <summary>
        /// Получить количество найденных в исходной строке повторений строки <paramref name="searchString"/>.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="searchString">Искомая строка.</param>
        public static int GetSubstringCount(this string text, string searchString)
        {
            if (text.IsNullOrEmpty() || searchString.IsNullOrEmpty())
                return 0;
            
            int index = 0;
            int subStrCount = 0;
            CompareInfo compareInfo = CultureInfo.InvariantCulture.CompareInfo;
            do
            {
                index = compareInfo.IndexOf(text, searchString, index, CompareOptions.Ordinal);
                if (index == -1) 
                    break;
                
                subStrCount++;
                index += searchString.Length;
            } while (index < text.Length);

            return subStrCount;
        }

        /// <summary>
        /// Удаление из строки всех вхождений указанных символов.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="charsToRemove">Список символов для удаления.</param>
        /// <param name="charComparer">Компаратор символов в строке (по умолчанию чувствительный к регистру).</param>
        /// <returns>Строка, не содержащая указанных символов.</returns>
        public static string RemoveChars(this string text, IEnumerable<char> charsToRemove, 
            IEqualityComparer<char> charComparer = null)
        {
            const int THRESHOLD = 6;

            if (text == null)
                throw new ArgumentNullException(nameof(text));
            if (charsToRemove == null)
                throw new ArgumentNullException(nameof(charsToRemove));

            char[] exceptArray = charsToRemove as char[] ?? charsToRemove.ToArray();

            char[] result = new char[text.Length];
            int wi = 0;
            if (exceptArray.Length > THRESHOLD)
            {
                HashSet<char> exceptSet = charComparer == null 
                    ? new HashSet<char>(exceptArray) 
                    : new HashSet<char>(exceptArray, charComparer);

                foreach (char c in text)
                    if (!exceptSet.Contains(c))
                        result[wi++] = c;
            }
            else
            {
                if (charComparer == null)
                {
                    charComparer = EqualityComparer<char>.Default;
                }

                foreach (char c in text)
                {
                    bool keepSymbol = true;
                    // ReSharper disable once LoopCanBeConvertedToQuery
                    foreach (char exceptSymbol in exceptArray)
                        if (charComparer.Equals(c, exceptSymbol))
                        {
                            keepSymbol = false;
                            break;
                        }

                    if (keepSymbol)
                        result[wi++] = c;
                }
            }

            return new string(result, 0, wi);
        }

        /// <summary>
        /// Удаление из строки всех вхождений указанных символов.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="charsToRemove">Список символов для удаления.</param>
        /// <returns>Строка, не содержащая указанных символов.</returns>
        public static string RemoveChars(this string text, params char[] charsToRemove)
        {
            return RemoveChars(text, (IEnumerable<char>)charsToRemove);
        }

        /// <summary>
        /// Удаление символов перевода строки из конца текста, если они есть.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="isAllDelete">Признак того, что нужно удалить все символы.
        /// Если равен false - удаляется только последний символ.</param>
        /// <returns>Измененный текст.</returns>
        public static string RemoveTerminatingNewlineChars(this string text, bool isAllDelete = true)
        {
            return new StringBuilder(text).RemoveTerminatingNewlineChars(isAllDelete).ToString();
        }
        
        /// <summary>
        /// Удаление пробелов из строки.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        public static string RemoveWhiteSpace(this string text)
        {
            StringBuilder sb = new StringBuilder();
            if (text == null) 
                return sb.ToString();
            
            foreach (char c in text.Where(c => !char.IsWhiteSpace(c)))
            {
                sb.Append(c);
            }
            
            return sb.ToString();
        }    
        
        /// <summary>
        /// Проверка строки на содержание только латинских символов.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <returns>Результат проверки.</returns>
        public static bool OnlyLatinChars(this string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            return text.All(c => (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'));
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
                StringBuilder builder = new StringBuilder();
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
        /// Преобразование строки в <see cref="double"/>, с заменой разделителя '.' или ','
        /// на значение разделителя в текущей локализации ОС.
        /// </summary>
        /// <param name="stringValue">Исходная строка.</param>
        /// <param name="defaultValue">Значение результата в случае ошибки преобразования.</param>
        public static double ParseToDouble(this string stringValue, double defaultValue = double.NaN)
        {
            if (stringValue == null)
                throw new ArgumentNullException(nameof(stringValue));
            
            CommonPhrases.Culture = CultureInfo.CurrentUICulture;       // устанавливаем яз. стандарт для фраз

            string normalized = stringValue.NormalizeForDouble();
            if (!string.IsNullOrEmpty(normalized))
            {
                if (double.TryParse(normalized, out double result))
                {
                    return result;
                }
            }

            if (defaultValue.Equals(double.NaN))
            {
                throw new ArgumentException(CommonPhrases.Exception_ParamIsNotDouble.Format(stringValue));
            }

            return defaultValue;
        }

        /// <summary>
        /// Замена разделителя '.' или ',' в текстовом представлении числа на
        /// значение разделителя в текущей локализации ОС.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <param name="culture">Языковая культура.</param>
        public static string NormalizeForDouble(this string value, CultureInfo culture = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            culture ??= CultureInfo.CurrentCulture;

            return value.Replace(".", culture.NumberFormat.NumberDecimalSeparator)
                .Replace(",", culture.NumberFormat.CurrencyGroupSeparator == ","
                    ? string.Empty
                    : culture.NumberFormat.NumberDecimalSeparator);
        }

        /// <summary>
        /// Преобразование строки в число. 
        /// </summary>
        /// <param name="stringValue">Исходная строка.</param>
        /// <param name="defaultValue">Значение результата в случае ошибки преобразования.</param>
        public static int ParseToInt(this string stringValue, int defaultValue = 0)
        {
            return int.TryParse(stringValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value)
                ? value
                : defaultValue;
        }

        /// <summary>
        /// Получение массива байт из строки.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <param name="encoding">Кодировка исходной строки.</param>
        public static byte[] ToByteArray(this string value, Encoding encoding = null)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            encoding ??= Encoding.UTF8;
            
            return encoding.GetBytes(value);
        }

        /// <summary>
        /// Получение строки из байт.
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        /// <param name="encoding">Кодировка получаемой строки.</param>
        public static string FromByteArray(this byte[] array, Encoding encoding = null)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            
            encoding ??= Encoding.UTF8;

            return encoding.GetString(array);
        }
        
        /// <summary>
        /// Получение массива ushort из строки.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        public static ushort[] ToUshortArray(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            IList<byte> bytes = value.ToByteArray().AddEmptyToEven();
            List<ushort> ushorts = new List<ushort>();
            for (int i = 0; i < bytes.Count; i += 2)
            {
                ushorts.Add((ushort)((bytes[i + 1] << 8) | bytes[i]));
            }

            return ushorts.ToArray();
        }

        /// <summary>
        /// Получение строки из массива ushort.
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        public static string FromUshortArray(this ushort[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            List<byte> bytes = new List<byte>();
            foreach (ushort val in array)
            {
                bytes.Add((byte)val);
                bytes.Add((byte)(val >> 8));
            }

            return bytes.ToArray().FromByteArray();
        }

        /// <summary>
        /// Дополнение байтов до четного количества пустым символом строки.
        /// </summary>
        /// <param name="array">Исходный массив.</param>
        public static IList<byte> AddEmptyToEven(this IEnumerable<byte> array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            List<byte> list = new List<byte>(array);
            if (list.Count % 2 != 0)
                list.Add(32); // " " Дополняем до четного пустым символом
            return list;
        }

        /// <summary>
        /// Добавление символов до указанной длины строки.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <param name="appendSymbol">Добавляемые символы.</param>
        /// <param name="length">Конечная длина строки.</param>
        public static string Append(this string value, int length, char appendSymbol = ' ')
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            int sourceLength = value.Length;

            if (length < sourceLength)
                return value.Substring(0, length);

            string appendString = new string(appendSymbol, length - sourceLength);

            return $"{value}{appendString}";
        }

        /// <summary>
        /// Удаление из строки цифрового окончания.
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        public static string RemoveNumericPostfix(this string value)
        {
            StringBuilder builder = new StringBuilder(value);
            for (int i = builder.Length - 1; i >= 0; i--)
            {
                if (!char.IsDigit(builder[i]))
                    break;

                builder.Remove(i, 1);
            }

            return builder.ToString();
        }
        
        /// <summary>
        /// Разделяет строку на пару "ключ - значение", используя разделитель delimiter (по умолчанию - ":").
        /// </summary>
        public static KeyValuePair<string, string> ToPairedValue(this string str, char delimiter = ':')
        {
            string[] splitedArray = str.Split(delimiter);
            return new KeyValuePair<string, string>(splitedArray[0], splitedArray[1]);
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
        /// Получение форматированной строки, путем замены элементов формата в строке format
        /// соответствующими элементами массива args. 
        /// </summary>
        /// <remarks>
        /// 1) В отформатированной строке удаляются все пустые подстроки (т.е. идущие подряд кавычки).
        /// <para>2) Если параметр isTrim == true, то в отформатированной строке удаляются незначащие пробелы.</para>
        /// </remarks>
        public static string FormatWithQuotes(this string format, bool isTrim = true, params object[] args)
        {
            if (format == null || args == null)
                throw new ArgumentNullException(format == null ? nameof(format) : nameof(args));

            StringBuilder builder = new StringBuilder(format.Length + args.Length * 8);
            builder.AppendFormat(format, args);
            builder.Replace(" \"\"", string.Empty);
            builder.Replace("\"\" ", string.Empty);
            builder.Replace("\"\"", string.Empty);
            builder.Replace(" «»", string.Empty);
            builder.Replace("«» ", string.Empty);
            builder.Replace("«»", string.Empty);
            
            return isTrim
                ? builder.ToString().Trim()
                : builder.ToString();
        }

        /// <summary>
        /// Вычисление размера (ширины) текста в px (или в pt, если установлен флаг isResultInPt).
        /// <para> ПРИМЕЧАНИЕ: Если использовать полученный результат для Excel XML - то будет вычислено не очень точно! </para>
        /// </summary>
        /// <param name="text"> Исходная строка. </param>
        /// <param name="font"> Шрифт исходного текста. </param>
        /// <param name="isResultInPt"> Флаг, если true - результат в px, если false - результат в pt. </param>
        /// <remarks>
        /// Поддерживается только Windows.
        /// </remarks>
        /*public static double MeasureText(this string text, Font font, bool isResultInPt = false)
        {
            double PxToPtX(int px, float dpiX)
            {
                return px * 72.0 / dpiX;
            }

            /* Все это, включая graphics.MeasureString, таботает не точно,
             * если использовать полученный результат для Excell XML !!! #1#

            using var g = Graphics.FromHwnd(IntPtr.Zero);
            g.TextRenderingHint = TextRenderingHint.SingleBitPerPixel;      // .AntiAlias;
            var sf = new StringFormat();                                    // StringFormat.GenericTypographic
            var sizeF = g.MeasureString(text, font, int.MaxValue, sf);
            double measure = sizeF.Width;

            if (isResultInPt)
                // Px -> pt
                measure = PxToPtX((int)measure, g.DpiX);

            return measure;
        }*/

        /// <summary>
        /// Выполняет делегат <see cref="Func{T, TResult}"/> над исходной строкой.
        /// </summary>
        /// <param name="text">Исходная строка.</param>
        /// <param name="func">Делегат.</param>
        /// <returns>Результирующая строка.</returns>
        public static string Func(this string text, Func<string, string> func = null)
        {
            return (func == null)
                ? text
                : func(text);
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
        
        /// <summary>
        /// Поиск совпадений в строках с учетом StringComparison.
        /// </summary>
        /// <param name="source">Исходная строка.</param>
        /// <param name="toCheck">Сравниваемая строка.</param>
        /// <param name="comp">Способ (компаратор) сравнения.</param>
        public static bool IsContains(this string source, string toCheck, 
            StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
        {
            if (source.IsNullOrEmpty())
                return toCheck.IsNullOrEmpty();

            return source.IndexOf(toCheck, comp) >= 0;
        }

        /// <summary>
        /// Проверка строк с учетом StringComparison.
        /// </summary>
        /// <param name="source">Исходная строка.</param>
        /// <param name="other">Сравниваемая строка.</param>
        /// <param name="comp">Способ (компаратор) сравнения.</param>
        public static bool IsEquals(this string source, string other, 
            StringComparison comp = StringComparison.InvariantCultureIgnoreCase)
        {
            if (ReferenceEquals(source, null))
                return ReferenceEquals(other, null);

            if (ReferenceEquals(other, null))
                return false;

            return source.Equals(other, comp);
        }
        
        
        /// <summary>
        /// Замена '.' или ',' на <see cref="CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator"/>
        /// </summary>
        /// <param name="value">Исходная строка.</param>
        /// <returns></returns>
        public static string NormalizeToDouble(this string value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            return value.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator)
                .Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        }
    }
}