using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Common.Core.Components;
using Infrastructure.Domain.Logging.Enums;

namespace Infrastructure.Interfaces.Logging
{
    /// <summary>
    /// Интерфейс для записи/чтения ILogItem сущностями.
    /// </summary>
    public interface ILogReaderWriter<T> where T : ILogItem
    {
        /// <summary>
        /// Получить языковую культуру.
        /// </summary>
        public CultureInfo Culture { get; }

        /// <summary>
        /// Получить кодировку лога.
        /// </summary>
        public Encoding Encoding { get; }

        /// <summary>
        /// Получить формат лога.
        /// </summary>
        public LogFormatEnum Format { get; }

        /// <summary>
        /// Признак успешной инициализации.
        /// </summary>
        bool IsInitSuccess { get; }

        /// <summary>
        /// Строка разделителя.
        /// </summary>
        string SeparatorString { get; set; }

        /// <summary>
        /// Инициализация "записывателя/читателя" лога.
        /// </summary>
        /// <param name="path">Путь к файлу лога.</param>
        /// <param name="logName">Имя лога.</param>
        /// <param name="format">Формат лога (простой, подробный).</param>
        /// <param name="encoding">Кодировка файла лога.</param>
        /// <param name="culture">Языковая культура (для форматирования даты/времени).</param>
        Result<bool> Init(string path, string logName = null, LogFormatEnum format = LogFormatEnum.Full, Encoding encoding = null,
            CultureInfo culture = null);

        /// <summary>
        /// Записать элемент лога.
        /// </summary>
        /// <param name="item">Записываемый элемент лога.</param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        Result<bool> Write(T item, object auxData = null);

        /// <summary>
        /// Записать разделитель.
        /// </summary>
        Result<bool> WriteSeparator();

        /// <summary>
        /// Прочитать элементы лога, с использованием фильтрации по порядковому номеру элементов.
        /// </summary>
        /// <param name="beginNumber">Начальный порядковый номер диапазона фильтрации.</param>
        /// <param name="endNumber">Конечный порядковый номер диапазона фильтрации.</param>
        /// <param name="result">Результат операции.</param>
        /// <returns>
        /// Список кортежей, где первый элемент кортежа - стандартный элемент лога (ILogItem),
        /// второй - дополнительные данные.
        /// </returns>
        IList<(T logItem, object auxData)> Read(int beginNumber, int endNumber, out Result<bool> result);

        /// <summary>
        /// Прочитать элементы лога, с использованием фильтрации по времени создания элементов.
        /// </summary>
        /// <param name="beginDt">Начальное время диапазона фильтрации.</param>
        /// <param name="endDt">Конечное время диапазона фильтрации.</param>
        /// <param name="result">Результат операции.</param>
        /// <returns>
        /// Список кортежей, где первый элемент кортежа - стандартный элемент лога (ILogItem),
        /// второй - дополнительные данные.
        /// </returns>
        IList<(T logItem, object auxData)> Read(DateTime beginDt, DateTime endDt, out Result<bool> result);
    }
}