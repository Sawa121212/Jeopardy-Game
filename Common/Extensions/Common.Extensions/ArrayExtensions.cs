using System;
using System.Globalization;
using Common.Extensions.Properties;

namespace Common.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="Array" />.
    /// </summary>
    public static class ArrayExtensions
    {
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
    }
}
