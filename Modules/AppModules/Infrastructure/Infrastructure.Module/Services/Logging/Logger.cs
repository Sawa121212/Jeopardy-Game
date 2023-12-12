using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Common.Core.Components;
using Common.Core.IO;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Interfaces.Logging;
using Infrastructure.Module.Services.Logging.ReadersWriters;

namespace Infrastructure.Module.Services.Logging
{
    /// <summary>
    /// Логгер (журнал) приложения.
    /// </summary>    
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Logger : ILogger
    {
        /// <summary>
        /// "Записыватель/чтец" лога.
        /// </summary>
        private readonly ILogReaderWriter<ILogItem> _readerWriter;

        /// <summary>
        /// Менеджер файлов для контроля устаревших файлов.
        /// </summary>
        private FileController _fileController;

        /// <inheritdoc />
        public LogItemCategoryEnum AllowedCategories { get; private set; }

        /// <inheritdoc/>
        public bool IsInitSuccess { get; private set; }

        /// <inheritdoc />
        public string Name { get; private set; }

        /// <inheritdoc />
        public string SeparatorString
        {
            get => _readerWriter.SeparatorString;
            private set => _readerWriter.SeparatorString = value;
        }

        /// <inheritdoc />
        public Result<bool> LastOperationResult { get; private set; }


        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logReaderWriterType">Тип "записывателя/чтеца" логов.</param>
        /// <param name="loggerName">Наименование логгера.</param>
        public Logger(LogReaderWriterTypeEnum logReaderWriterType, string loggerName = null)
            : this(CreateReaderWriter(logReaderWriterType), loggerName)
        {
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="logReaderWriter">"Записыватель/чтец" логов.</param>
        /// <param name="loggerName">Наименование логгера.</param>
        public Logger(ILogReaderWriter<ILogItem> logReaderWriter, string loggerName = null)
        {
            _readerWriter = logReaderWriter;
            AllowedCategories = LogItemCategoryEnum.All;
            IsInitSuccess = false;
            Name = string.IsNullOrEmpty(loggerName)
                ? "Logger"
                : loggerName;
        }

        /// <summary>
        /// Создать "записыватель/чтец" лога (фабричный метод).
        /// </summary>
        protected static ILogReaderWriter<ILogItem> CreateReaderWriter(LogReaderWriterTypeEnum logReaderWriterType)
        {
            return logReaderWriterType switch
            {
                LogReaderWriterTypeEnum.Text => new TextReaderWriter(),
                LogReaderWriterTypeEnum.Csv => new CsvReaderWriter(),
                LogReaderWriterTypeEnum.Console => new ConsoleReaderWriter(),
                _ => new TextReaderWriter()
            };
        }

        /// <inheritdoc />
        public Result<bool> Init(
            string path, string logName = null,
            LogItemCategoryEnum allowedCategories = LogItemCategoryEnum.All,
            LogFormatEnum format = LogFormatEnum.Full,
            Encoding encoding = null, CultureInfo culture = null)
        {
            AllowedCategories = allowedCategories;
            logName = string.IsNullOrEmpty(logName)
                ? $"{Name}_{DateTime.Today:yyyy.MM.dd}"
                : logName;

            Result<bool> result = _readerWriter.Init(path, logName, format, encoding, culture);
            IsInitSuccess = result.Value;

            return LastOperationResult = result;
        }

        /// <inheritdoc />
        public Result<bool> InitUsingFileControl(
            string path, string fileName = null,
            LogItemCategoryEnum allowedCategories = LogItemCategoryEnum.All,
            LogFormatEnum format = LogFormatEnum.Full,
            Encoding encoding = null, CultureInfo culture = null,
            int maxFileCount = 0, DateTime? oldestFileCreationDate = null, int maxCapacity = 0)
        {
            Result<bool> result = Init(path, fileName, allowedCategories, format, encoding, culture);
            if (result)
                _fileController = maxFileCount == 0 && oldestFileCreationDate is null && maxCapacity == 0
                    ? new FileController(path)
                    : new FileController(path, maxFileCount, oldestFileCreationDate ?? DateTime.MaxValue, maxCapacity);

            return LastOperationResult = result;
        }

        /// <inheritdoc />
        public void SetLoggerName(string loggerName)
        {
            Name = loggerName ?? Name;
        }

        /// <inheritdoc />
        public void SetAllowedCategories(LogItemCategoryEnum allowedCategories)
        {
            AllowedCategories = allowedCategories;
        }

        /// <inheritdoc />
        public void SetSeparatorString(string separatorString)
        {
            SeparatorString = separatorString;
        }

        /// <inheritdoc />
        public ILogger WriteLogItem(ILogItem logItem, object auxData = null)
        {
            if (AllowedCategories.HasFlag(logItem.Category))
            {
                LastOperationResult = _readerWriter.Write(logItem, auxData);
                _fileController?.CheckAll();
                _fileController?.RemoveOldFiles();
            }

            return this;
        }

        /// <inheritdoc />
        public ILogger WriteDebugItem(ILogItem logItem, object auxData = null)
        {
            logItem.SetCategory(LogItemCategoryEnum.Debug);
            if (AllowedCategories.HasFlag(LogItemCategoryEnum.Debug))
            {
                LastOperationResult = _readerWriter.Write(logItem, auxData);
                _fileController?.CheckAll();
                _fileController?.RemoveOldFiles();
            }

            return this;
        }

        /// <inheritdoc />
        public ILogger WriteExceptionItem(ILogItem logItem, object auxData = null)
        {
            logItem.SetCategory(LogItemCategoryEnum.Exception);
            if (AllowedCategories.HasFlag(LogItemCategoryEnum.Exception))
            {
                LastOperationResult = _readerWriter.Write(logItem, auxData);
                _fileController?.CheckAll();
                _fileController?.RemoveOldFiles();
            }

            return this;
        }

        /// <inheritdoc />
        public ILogger WriteInfoItem(ILogItem logItem, object auxData = null)
        {
            logItem.SetCategory(LogItemCategoryEnum.Info);
            if (AllowedCategories.HasFlag(LogItemCategoryEnum.Info))
            {
                LastOperationResult = _readerWriter.Write(logItem, auxData);
                _fileController?.CheckAll();
                _fileController?.RemoveOldFiles();
            }

            return this;
        }

        /// <inheritdoc />
        public ILogger WriteWarningItem(ILogItem logItem, object auxData = null)
        {
            logItem.SetCategory(LogItemCategoryEnum.Warning);
            if (AllowedCategories.HasFlag(LogItemCategoryEnum.Warning))
            {
                LastOperationResult = _readerWriter.Write(logItem, auxData);
                _fileController?.CheckAll();
                _fileController?.RemoveOldFiles();
            }

            return this;
        }

        /// <inheritdoc />
        public ILogger WriteErrorItem(ILogItem logItem, object auxData = null)
        {
            logItem.SetCategory(LogItemCategoryEnum.Error);
            if (AllowedCategories.HasFlag(LogItemCategoryEnum.Error))
            {
                LastOperationResult = _readerWriter.Write(logItem, auxData);
                _fileController?.CheckAll();
                _fileController?.RemoveOldFiles();
            }

            return this;
        }

        /// <inheritdoc />
        public ILogger WriteSeparator(string newSeparatorString = null)
        {
            LastOperationResult = _readerWriter.WriteSeparator();
            _fileController?.CheckAll();
            _fileController?.RemoveOldFiles();

            return this;
        }

        /// <inheritdoc />
        public IList<(ILogItem logItem, object auxData)> Read(int beginNumber, int endNumber)
        {
            IList<(ILogItem logItem, object auxData)> valueTuples = _readerWriter.Read(beginNumber, endNumber, out Result<bool> result);
            LastOperationResult = result;

            return valueTuples;
        }

        /// <inheritdoc />
        public IList<(ILogItem logItem, object auxData)> Read(DateTime beginDt, DateTime endDt)
        {
            IList<(ILogItem logItem, object auxData)> valueTuples = _readerWriter.Read(beginDt, endDt, out Result<bool> result);
            LastOperationResult = result;

            return valueTuples;
        }
    }
}