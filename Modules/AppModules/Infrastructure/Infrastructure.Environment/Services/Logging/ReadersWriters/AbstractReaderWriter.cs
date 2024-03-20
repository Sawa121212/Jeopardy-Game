using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using Common.Core.Components;
using Common.Core.Components.Exceptions;
using Common.Extensions;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Interfaces.Logging;

namespace Infrastructure.Environment.Services.Logging.ReadersWriters
{
    /// <summary>
    /// Абстрактный класс для записи/чтения лога.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
    public abstract class AbstractReaderWriter : ILogReaderWriter<ILogItem>
    {
        /// <summary>
        /// Объект для синхронизации обращения к журналу из разных потоков.
        /// </summary>
        /// <remarks>
        /// Актуально только для ReaderWriter'ов, работающих с файловой системой.
        /// </remarks>
        protected static readonly Mutex LockerMutex = new();

        /// <summary>
        /// Предыдущее значение записи лога.
        /// </summary>
        protected ILogItem previousLogItem;

        /// <inheritdoc/>
        public CultureInfo Culture { get; protected set; }

        /// <inheritdoc/>
        public Encoding Encoding { get; protected set; }

        /// <inheritdoc/>
        public LogFormatEnum Format { get; protected set; }

        /// <inheritdoc/>
        public bool IsInitSuccess { get; protected set; }

        private string _separatorString;
        /// <inheritdoc/>
        public string SeparatorString
        {
            get => _separatorString;
            set => _separatorString = value.IsNullOrEmpty()
                    ? new string('-', 50)
                    : value;
        }


        /// <summary>
        /// Конструктор.
        /// </summary>
        protected AbstractReaderWriter()
        {
            previousLogItem = new LogItem(0, DateTime.Now, string.Empty);
            SeparatorString = null;
        }

        /// <summary>
        /// Получить исключение, означающее неудачную инициализацию.
        /// </summary>
        protected Exception GetBadInitException() =>
            BaseException.CreateException(
                $"For {nameof(TextReaderWriter)} to work, it is necessary to initialize the storage path.",
                null,
                "ru",
                $"Для работы {nameof(TextReaderWriter)} необходимо инициализировать путь для хранения."
                );

        /// <summary>
        /// Получить количество строк в файле.
        /// </summary>
        /// <param name="fullFileName">Полное имя файла.</param>
        /// <param name="encoding">Кодировка файла.</param>
        /// <param name="exception">Ошибка, если произошла, иначе - null.</param>
        /// <remarks>
        /// Проверка ошибок и параметров не осуществляется!
        /// </remarks>
        protected static int GetLineCount(string fullFileName, Encoding encoding, out Exception exception)
        {
            exception = null;
            LockerMutex.WaitOne();
            try
            {
                using FileStream stream = File.OpenRead(fullFileName);
                return stream.GetNewlineCharCount(encoding);
            }
            catch (Exception ex)
            {
                exception = ex;
                return -1;
            }
            finally
            {
                LockerMutex.ReleaseMutex();
            }
        }

        /// <summary>
        /// Получить заголовок лога.
        /// </summary>
        /// <param name="isSimpleFormat">Признак простого формата логгера.</param>
        /// <param name="columnSeparator">Разделитель столбцов.</param>
        protected static string GetHeaderString(bool isSimpleFormat, string columnSeparator)
        {
            return isSimpleFormat
                ? $"{nameof(LogItem.Number)}{columnSeparator}{nameof(LogItem.TimeStamp)}{columnSeparator}" +
                        $"{nameof(LogItem.Message)}"
                : $"{nameof(LogItem.Number)}{columnSeparator}{nameof(LogItem.TimeStamp)}{columnSeparator}" +
                        $"{nameof(LogItem.CompName)}{columnSeparator}{nameof(LogItem.UserName)}{columnSeparator}" +
                        $"{nameof(LogItem.AppName)}{columnSeparator}{nameof(LogItem.SourceMethod)}{columnSeparator}" +
                        $"{nameof(LogItem.Category)}{columnSeparator}{nameof(LogItem.Message)}";
        }

        /// <summary>
        /// Записать разделитель в лог-файл.
        /// </summary>
        /// <remarks>
        /// Актуально только для ReaderWriter'ов, работающих с файловой системой.
        /// </remarks>
        protected Result<bool> WriteSeparatorToLogFile(string fullFileName,
            string columnSeparator, string separatorStringFormat)
        {
            int lineCount = 2;                                                      // количество строк в файле лога
            bool isExist = File.Exists(fullFileName);
            if (isExist)
                lineCount = GetLineCount(fullFileName, Encoding, out Exception _) + 1;    // возможно, нужно учесть ошибку

            LockerMutex.WaitOne();
            try
            {
                using StreamWriter sw = new(fullFileName!, isExist, Encoding);

                // Если файла не существовало - печатаем заголовок
                if (!isExist)
                    sw.WriteLine(GetHeaderString(Format == LogFormatEnum.Simple, columnSeparator));

                sw.WriteLine(separatorStringFormat, lineCount, SeparatorString);

                sw.Flush();
                sw.Close();
            }
            catch (Exception ex)
            {
                // Проблемы с файлом.
#if DEBUG
                Console.WriteLine($"--- {ex.Message}");
#endif
                return Result<bool>.Fail(ex);
            }
            finally
            {
                LockerMutex.ReleaseMutex();
            }

            return Result<bool>.Done(true);
        }

        /// <inheritdoc />
        public abstract Result<bool> Init(string path, string logName = null,
            LogFormatEnum format = LogFormatEnum.Full, Encoding encoding = null, CultureInfo culture = null);

        /// <inheritdoc />
        public abstract Result<bool> Write(ILogItem logItem, object auxData = null);

        /// <inheritdoc />
        public abstract Result<bool> WriteSeparator();

        /// <inheritdoc />
        public abstract IList<(ILogItem logItem, object auxData)>
            Read(DateTime beginDt, DateTime endDt, out Result<bool> result);

        /// <inheritdoc />
        public abstract IList<(ILogItem logItem, object auxData)>
            Read(int beginNumber, int endNumber, out Result<bool> result);
    }
}