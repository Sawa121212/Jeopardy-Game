using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Common.Extensions.Properties;

namespace Common.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="Array" />.
    /// </summary>
    public static class ArrayExtensions
    {
        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static readonly Random RANDOM = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// Получение массива случайных символов.
        /// </summary>
        /// <param name="capacity">Вместимость.</param>
        public static char[] RandomCharArray(int capacity)
        {
            char[]? arr = new char[capacity];
            for (int i = 0; i < arr.Length; i++)
            {
                int num = RANDOM.Next(Int16.MaxValue);
                arr[i] = Convert.ToChar(num);
            }

            return arr;
        }
        
        /// <summary>
        /// Получение массива целых положительных чисел.
        /// </summary>
        /// <param name="capacity">Вместимость.</param>
        public static int[] RandomIntArray(int capacity)
        {
            int[]? arr = new int[capacity];
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i] = RANDOM.Next();
            }

            return arr;
        }

        /// <summary>
        /// Проверка массива на наличие в нем элементов.
        /// </summary>
        /// <typeparam name="T">Тип элементов коллекции.</typeparam>
        /// <param name="source">Коллекция.</param>
        /// <returns>Признак, есть ли в коллекции элементы.</returns>
        /// <exception cref="ArgumentNullException" />
        public static bool IsEmpty<T>(this T[] source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Length == 0;
        }

        /// <summary>
        /// Получить копию части массива.
        /// </summary>
        /// <typeparam name="T">Тип элементов массива.</typeparam>
        /// <param name="data">Исходный массив.</param>
        /// <param name="index">Индекс первого выбираемого элемента в исходном массиве.</param>
        /// <param name="length">Количество выбираемых элементов из исходного массива.</param>
        public static T[] SubArray<T>(this T[] data, long index, long length)
        {
            CommonPhrases.Culture = CultureInfo.CurrentUICulture;       // устанавливаем яз. стандарт для фраз

            if (data.Length < length + index)
                throw new ArgumentException(
                    CommonPhrases.Exception_ArrayBoundsError);
            
            T[]? result = new T[length];
            Array.Copy(data, index, result, 0, length);
            
            return result;
        }
        
        /// <summary>
        /// Преобразовать массив байт в строку.
        /// </summary>
        public static string ToStr(this byte[] bytes, int startIndex = 0, int length = int.MaxValue)
        {
            // length = bytes[startIndex];
            // startIndex++;
            if ((uint)startIndex + length > bytes.Length)
                length = bytes.Length - startIndex;
            return Encoding.Default.GetString(bytes, startIndex, length);
        }
    }
}
