using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Common.Core.Components;
using Infrastructure.Domain.Logging.Enums;

namespace Infrastructure.Interfaces.Logging
{
    /// <summary>
    /// Интерфейс Логгера (журнала) приложения.
    /// </summary>    
    public interface ILogger
    {
        /// <summary>
        /// Разрешенные категории.
        /// </summary>
        LogItemCategoryEnum AllowedCategories { get; }
        
        /// <summary>
        /// Признак успешной инициализации.
        /// </summary>
        public bool IsInitSuccess { get; }
        
        /// <summary>
        /// Название логгера.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Строка-разделитель.
        /// </summary>
        string SeparatorString { get; }
        
        /// <summary>
        /// Результат выполнения последней операции.
        /// </summary>
        Result<bool> LastOperationResult { get; }

        /// <summary>
        /// Инициализация.
        /// </summary>
        Result<bool> Init(string path, string logName = null, 
            LogItemCategoryEnum allowedCategories = LogItemCategoryEnum.All, 
            LogFormatEnum format = LogFormatEnum.Full, Encoding encoding = null, CultureInfo culture = null);

        /// <summary>
        /// Инициализация.
        /// </summary>
        Result<bool> InitUsingFileControl(string path,  string fileName = null, 
            LogItemCategoryEnum allowedCategories = LogItemCategoryEnum.All,
            LogFormatEnum format = LogFormatEnum.Full, Encoding encoding = null, CultureInfo culture = null,
            int maxFileCount = 0, DateTime? oldestFileCreationDate = null, int maxCapacity = 0);

        /// <summary>
        /// Устанавливаем имя логгера.
        /// </summary>
        public void SetLoggerName(string loggerName);
        
        /// <summary>
        /// Устанавливаем разрешенные категории.
        /// </summary>
        void SetAllowedCategories(LogItemCategoryEnum allowedCategories);

        /// <summary>
        /// Устанавливаем строку-разделитель.
        /// </summary>
        void SetSeparatorString(string separatorString);
        
        /// <summary>
        /// Запись сообщения в лог; тип сообщения определяется
        /// <see cref="ILogItem.Category">logItem.Category</see>.
        /// </summary>
        /// <param name="logItem">Записываемый элемент лога <see cref="ILogItem"/></param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        ILogger WriteLogItem(ILogItem logItem, object auxData = null);

        /// <summary>
        /// Запись отладочного сообщения в лог.
        /// </summary>
        /// <param name="logItem">Записываемый элемент лога <see cref="ILogItem"/></param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        ILogger WriteDebugItem(ILogItem logItem, object auxData = null);

        /// <summary>
        /// Запись сообщения об исключении в лог.
        /// </summary>
        /// <param name="logItem">Записываемый элемент лога <see cref="ILogItem"/></param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        ILogger WriteExceptionItem(ILogItem logItem, object auxData = null);

        /// <summary>
        /// Запись информационного сообщения.
        /// </summary>
        /// <param name="logItem">Записываемый элемент лога <see cref="ILogItem"/></param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        ILogger WriteInfoItem(ILogItem logItem, object auxData = null);

        /// <summary>
        /// Запись предупреждающего сообщения.
        /// </summary>
        /// <param name="logItem">Записываемый элемент лога <see cref="ILogItem"/></param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        ILogger WriteWarningItem(ILogItem logItem, object auxData = null);
        
        /// <summary>
        /// Запись сообщения об ошибке в лог.
        /// </summary>
        /// <param name="logItem">Записываемый элемент лога <see cref="ILogItem"/></param>
        /// <param name="auxData">Дополнительные данные (при необходимости).</param>
        ILogger WriteErrorItem(ILogItem logItem, object auxData = null);

        /// <summary>
        /// Запись разделителя в лог.
        /// </summary>
        /// <param name="newSeparatorString">Строка разделителя, которая будет использоваться в дальнейшем.
        /// Если null - используется стандартная строка разделителя.</param>
        ILogger WriteSeparator(string newSeparatorString = null);
        
        /// <summary>
        /// Прочитать элементы лога, с использованием фильтрации по порядковому номеру элементов.
        /// </summary>
        /// <param name="beginNumber">Начальный порядковый номер диапазона фильтрации.</param>
        /// <param name="endNumber">Конечный порядковый номер диапазона фильтрации.</param>
        /// <returns>
        /// Список кортежей, где первый элемент кортежа - стандартный элемент лога (ILogItem),
        /// второй - дополнительные данные.
        /// </returns>
        IList<(ILogItem logItem, object auxData)> Read(int beginNumber, int endNumber);
        
        /// <summary>
        /// Прочитать элементы лога, с использованием фильтрации по времени создания элементов.
        /// </summary>
        /// <param name="beginDt">Начальное время диапазона фильтрации.</param>
        /// <param name="endDt">Конечное время диапазона фильтрации.</param>
        /// <returns>
        /// Список кортежей, где первый элемент кортежа - стандартный элемент лога (ILogItem),
        /// второй - дополнительные данные.
        /// </returns>
        IList<(ILogItem logItem, object auxData)> Read(DateTime beginDt, DateTime endDt);
    }
}
