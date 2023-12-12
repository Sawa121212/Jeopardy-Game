using System.Collections.Generic;

namespace Common.Extensions.Collections
{
    /// <summary>
    /// Методы расширения для классов <see cref="HashSet{T}"/>.
    /// </summary>
    public static class HashSetExtensions
    {
        /// <summary>
        /// Добавляет последовательность во множество.
        /// </summary>
        public static bool AddRange<T>(this HashSet<T> @this, IEnumerable<T> items)
        {
            bool allAdded = true;
            foreach (T? item in items)
            {
                allAdded &= @this.Add(item);
            }
            return allAdded;
        }
    }
}