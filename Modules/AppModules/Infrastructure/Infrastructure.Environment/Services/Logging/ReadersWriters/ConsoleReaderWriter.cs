using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Common.Core.Components;
using Common.Extensions;
using Common.Extensions.ValueTypes;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Interfaces.Logging;

namespace Infrastructure.Environment.Services.Logging.ReadersWriters
{
    /// <summary>
    /// Записывает лог в консоль.
    /// </summary>
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    internal class ConsoleReaderWriter : AbstractReaderWriter
    {
        /// <inheritdoc />
        /// <remarks>
        /// Параметр <paramref name="path"/> игнорируется
        /// (не имеет смысла в контексте работы с консолью).
        /// </remarks>
        public override Result<bool> Init(string path, string logName = null,
            LogFormatEnum format = LogFormatEnum.Full, Encoding encoding = null, CultureInfo culture = null)
        {
            Format = format;
            Encoding = encoding ?? Encoding.Default;
            Culture = culture ?? CultureInfo.CurrentCulture;
            
            return Result<bool>.Done(IsInitSuccess = true);
        }

        /// <inheritdoc />
        public override Result<bool> Write(ILogItem logItem, object auxData = null)
        {
            if (logItem == null) 
                return Result<bool>.Done(true);

            Encoding currConsoleEncoding = Console.OutputEncoding;
            
            if (!logItem.IsRepeatable)
            {
                if (previousLogItem.Equals(logItem))
                    return Result<bool>.Done(true);
                previousLogItem = logItem;
            }

            try
            {
                DateTime dt = logItem.TimeStamp;
                
                string str = (Format == LogFormatEnum.Simple)
                    ? $"{logItem.Number} {dt.StrFromDate(Culture)} {dt.StrFromTime(Culture)} {logItem.Message}"
                    : $"{logItem.Number} " +
                            $"{dt.StrFromDate(Culture)} {dt.StrFromTime(Culture)} " +
                            (logItem.CompName.IsNullOrEmpty() ? string.Empty : "<" + logItem.CompName + ">") +
                            (logItem.UserName.IsNullOrEmpty() ? string.Empty : "<" + logItem.UserName + ">") +
                            (logItem.AppName.IsNullOrEmpty() ? string.Empty : "<" + logItem.AppName + ">") +
                            (logItem.SourceMethod.IsNullOrEmpty() ? string.Empty : "<" + logItem.SourceMethod + ">") +
                            $"<{logItem.Category.ToString()}> " +
                            $"{logItem.Message}";
                Console.OutputEncoding = Encoding;
                Console.WriteLine(str);
            
                Console.OutputEncoding = currConsoleEncoding;
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex);
            }
            
            return Result<bool>.Done(true);
        }

        /// <inheritdoc />
        public override Result<bool> WriteSeparator()
        {
            try
            {
                Encoding currConsoleEncoding = Console.OutputEncoding;
                Console.OutputEncoding = Encoding;
                Console.WriteLine(SeparatorString);
                Console.OutputEncoding = currConsoleEncoding;

                return Result<bool>.Done(true);
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail(ex);
            }
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
