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

namespace Infrastructure.Environment.Services.Logging.ReadersWriters
{
    /// <summary>
    /// Записывает/читает лог в текстовый файл.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class TextReaderWriter : AbstractReaderWriter
    {
        /// <summary>
        /// Расширение файла лога.
        /// </summary>
        private const string Extension = "log";

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
        public TextReaderWriter()
        {
        }

        /// <inheritdoc />
        /// 
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

            Exception exception = null;
            bool isExist = File.Exists(FullFileName);
            int lineCount = isExist                                              // количество строк в файле лога
                ? GetLineCount(FullFileName, Encoding, out exception) + 1           // возможно, нужно учесть ошибку
                : 2;
#if DEBUG
            if (exception != null)
                Console.WriteLine($"- Не смогли прочитать кол-во строк {exception.Message}");
#endif

            LockerMutex.WaitOne();
            try
            {
                using StreamWriter sw = new(FullFileName, isExist, Encoding);

                logItem.SetNumber(lineCount);
                DateTime dt = logItem.TimeStamp;

                // Если файла не существовало - печатаем заголовок
                if (!isExist)
                    // TODO: переделать на асинхронные операции
                    sw.WriteLineAsync(GetHeaderString(Format == LogFormatEnum.Simple, "\t"));

                if (Format == LogFormatEnum.Simple)
                {
                    // Простой формат логгера
                    sw.WriteLine(
                        $"{logItem.Number,6} {dt.StrFromDate(Culture)} {dt.StrFromTime(Culture)} {logItem.Message}"
                    );
                }
                else
                {
                    // Полный формат логгера
                    sw.WriteLine(
                        $"{logItem.Number,6} " +
                        $"{dt.StrFromDate(Culture)} {dt.StrFromTime(Culture)} " +
                        (logItem.CompName.IsNullOrEmpty() ? string.Empty : "<" + logItem.CompName + ">") +
                        (logItem.UserName.IsNullOrEmpty() ? string.Empty : "<" + logItem.UserName + ">") +
                        (logItem.AppName.IsNullOrEmpty() ? string.Empty : "<" + logItem.AppName + ">") +
                        (logItem.SourceMethod.IsNullOrEmpty() ? string.Empty : "<" + logItem.SourceMethod + ">") +
                        $"<{logItem.Category.ToString()}> " +
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
                : WriteSeparatorToLogFile(FullFileName, "\t", "{0, 6} {1}");
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