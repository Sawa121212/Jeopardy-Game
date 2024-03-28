using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Common.Extensions.Collections
{
    /// <summary>
    /// Методы-расширения для <see cref="IEnumerable{T}" />.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Выполнение некоторого действия с элементами последовательности.
        /// </summary>
        /// <typeparam name="T">Тип элемента последовательности.</typeparam>
        /// <param name="source">Исходная последовательность.</param>
        /// <param name="action">Выполняемое действие.</param>
        /// <remarks>Если действие не указано, метод просто итерирует по всем элементам последовательности.</remarks>
        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action = null)
        {
            if (action == null)
            {
                // Ничего не делаем, только пробегаемся по списку
                using IEnumerator<T>? enumerator = source.GetEnumerator();

                while (enumerator.MoveNext())
                {
                }
            }
            else
            {
                foreach (T? element in source)
                    action(element);
            }
        }

        /// <summary>
        /// Удаление из последовательности пустых строк.
        /// </summary>
        public static IEnumerable<string> RemoveEmptyStr(this IEnumerable<string> source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            return source.Where(s => !s.IsNullOrWhiteSpace());
        }
    }
}