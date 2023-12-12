using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Text;
using Common.Core.Components;
using Common.Core.IO;
using Common.Extensions;
using Common.Extensions.ValueTypes;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Interfaces.Logging;

namespace Infrastructure.Module.Services.Logging.ReadersWriters
{
    /// <summary>
    /// Записывает лог в файл csv.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class CsvReaderWriter : AbstractReaderWriter
    {
        /// <summary>
        /// Расширение файла лога.
        /// </summary>
        private const string Extension = "csv";

        /// <summary>
        /// Объект для синхронизации обращения к журналу из разных потоков.
        /// </summary>
        private readonly object _locker;

        /// <summary>
        /// Путь к папке с логом.
        /// </summary>
        private string _logsFolder;

        /// <summary>
        /// Получить имя журнала.
        /// </summary>
        public string FullFileName { get; private set; }


        /// <summary>
        /// Конструктор.
        /// </summary>
        public CsvReaderWriter()
        {
            _locker = new object();
        }

        /// <inheritdoc />
        public override Result<bool> Init(string path, string logName = null,
            LogFormatEnum format = LogFormatEnum.Full, Encoding encoding = null, CultureInfo culture = null)
        {
            try
            {
                Directory.CreateDirectory(path);
            }
            catch (Exception e)
            {
                return Result<bool>.Fail(e);
            }

            _logsFolder = path;
            FullFileName = logName.IsNullOrEmpty()
                ? PathExtensions.Combine(_logsFolder, DateTime.Today.DateToFileName(Extension))
                : PathExtensions.Combine(_logsFolder, $"{logName}.{Extension}");
            Format = format;
            Encoding = encoding ?? Encoding.Default;
            Culture = culture ?? CultureInfo.CurrentCulture;

            return Result<bool>.Done(IsInitSuccess = true);
        }

        /// <inheritdoc />
        public override Result<bool> Write(ILogItem logItem, object auxData = null)
        {
            if (_logsFolder.IsNullOrEmpty())
                return Result<bool>.Fail(GetBadInitException());

            if (logItem == null)
                return Result<bool>.Done(true);

            if (!logItem.IsRepeatable)
            {
                if (previousLogItem.Equals(logItem))
                    return Result<bool>.Done(true);
                previousLogItem = logItem;
            }

            int lineCount = 2; // количество строк в файле лога
            bool isExist = File.Exists(FullFileName);
            if (isExist)
                lineCount = GetLineCount(FullFileName, Encoding, out Exception _) + 1; // возможно, нужно учесть ошибку

            LockerMutex.WaitOne();
            try
            {
                using StreamWriter sw = new StreamWriter(FullFileName, isExist, Encoding);

                logItem.SetNumber(lineCount);
                DateTime dt = logItem.TimeStamp;

                // Если файла не существовало - печатаем заголовок
                if (!isExist)
                    sw.WriteLine(GetHeaderString(Format == LogFormatEnum.Simple, ";"));

                if (Format == LogFormatEnum.Simple)
                {
                    // Простой формат логгера
                    sw.WriteLine(
                        $"{logItem.Number};{dt.StrFromDate(Culture)};{dt.StrFromTime(Culture)};{logItem.Message}"
                    );
                }
                else
                {
                    // Полный формат логгера
                    sw.WriteLine(
                        $"{logItem.Number};" +
                        $"{dt.StrFromDate(Culture)} {dt.StrFromTime(Culture)};" +
                        $"{logItem.CompName ?? string.Empty};" +
                        $"{logItem.UserName ?? string.Empty};" +
                        $"{logItem.AppName ?? string.Empty};" +
                        $"{logItem.SourceMethod ?? string.Empty};" +
                        $"{logItem.Category.ToString()};" +
                        $"{logItem.Message}"
                    );
                }

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
        public override Result<bool> WriteSeparator()
        {
            return _logsFolder.IsNullOrEmpty()
                ? Result<bool>.Fail(GetBadInitException())
                : WriteSeparatorToLogFile(FullFileName, ";", "{0};{1}");
        }

        /// <inheritdoc />
        public override IList<(ILogItem logItem, object auxData)>
            Read(DateTime beginDt, DateTime endDt, out Result<bool> result)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public override IList<(ILogItem logItem, object auxData)>
            Read(int beginNumber, int endNumber, out Result<bool> result)
        {
            throw new NotImplementedException();
        }
    }
}