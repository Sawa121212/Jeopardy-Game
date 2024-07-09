using System;
using System.Collections.Generic;

namespace Common.Extensions.Collections
{
    /// <summary>
    /// Методы-расширения для <see cref="IDictionary{TKey, TValu}" />.
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>
        /// Синтаксический сахар для получения значения из словаря.
        /// </summary>
        /// <typeparam name="TKey">Тип ключа.</typeparam>
        /// <typeparam name="TValue">Тип значения.</typeparam>
        /// <param name="dictionary">Словарь.</param>
        /// <param name="key">Ключ.</param>
        /// <returns>Значение из словаря.</returns>
        public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
        {
            if (dictionary == null)
                throw new ArgumentNullException(nameof(dictionary));

            dictionary.TryGetValue(key, out TValue? obj);
            return obj;
        }
    }
}