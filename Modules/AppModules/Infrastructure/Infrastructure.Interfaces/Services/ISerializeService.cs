using System;
using System.IO;

namespace Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Интерфейс сервиса сериализации
    /// </summary>
    public interface ISerializeService
    {
        /// <summary>
        /// Сериализует данный экземляр в массив байт.
        /// </summary>
        /// <param name="instance">Инстанс класса (cannot be null).</param>
        /// <returns>массив байт</returns>
        byte[] Serialize<T>(T instance);

        /// <summary>
        /// Сериализует данный экземляр в поток.
        /// </summary>
        /// <param name="instance">Инстанс класса (cannot be null).</param>
        /// <param name="destinationStream">Поток для записи</param>
        void Serialize<T>(Stream destinationStream, T instance);

        /// <summary>
        /// Сериализует данный экземляр в файл.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename">Имя файла.</param>
        /// <param name="instance">Инстанс класса (cannot be null).</param>
        void Serialize<T>(string filename, T instance);

        /// <summary>
        /// Десериализация файла.
        /// </summary>
        /// <typeparam name="T">Тип класса.</typeparam>
        /// <param name="filename">Имя файла.</param>
        /// <returns>Экземпляр класса типа T</returns>
        T Deserialize<T>(string filename);

        /// <summary>
        /// Десериализация потока
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <param name="source">Поток - источник (cannot be null).</param>
        /// <returns>Инстанс класса типа T</returns>
        T Deserialize<T>(Stream source);

        /// <summary>
        /// Десериализация потока
        /// </summary>
        /// <param name="type">Тип класса</param>
        /// <param name="source">Поток - источник (cannot be null).</param>
        /// <returns>Инстанс класса типа type</returns>
        object Deserialize(Type type, Stream source);

        /// <summary>
        /// Десериализация бинарного массива
        /// </summary>
        /// <typeparam name="T">Тип класса</typeparam>
        /// <param name="source">Массив - источник (cannot be null).</param>
        /// <returns>Инстанс класса типа T</returns>
        T Deserialize<T>(byte[] source);

        /// <summary>
        /// Десериализация бинарного массива
        /// </summary>
        /// <param name="type">Тип класса</param>
        /// <param name="source">Массив - источник (cannot be null).</param>
        /// <returns>Инстанс класса типа type</returns>
        object Deserialize(Type type, byte[] source);

        /// <summary>
        /// Слияние потока с инстансом класса типа T
        /// </summary>
        /// <typeparam name="T">Тип класса.</typeparam>
        /// <param name="instance">Инстанс класса (can be null).</param>
        /// <param name="source">Поток - источник (cannot be null).</param>
        /// <returns>Измененный инстанс класса; может отличаться от аргумента экземпляра,
        /// если исходный экземпляр равен NULL, 
        /// или поток определяет известный подтип исходного экземпляра.</returns>
        T Merge<T>(Stream source, T instance);
    }
}