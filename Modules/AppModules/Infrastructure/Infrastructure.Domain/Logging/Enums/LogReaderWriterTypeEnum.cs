using System;
using System.ComponentModel;

namespace Infrastructure.Domain.Logging.Enums
{
    /// <summary>
    /// Типы "записывателей/читателей" логов.
    /// </summary>
    [Serializable]
    public enum LogReaderWriterTypeEnum
    {
        /// <summary>
        /// Текстовый файл.
        /// </summary>
        [Description("Текстовый файл")]
        Text = 1,
        
        /// <summary>
        /// Файл CSV.
        /// </summary>
        [Description("Файл CSV")]
        Csv,
        
        /// <summary>
        /// Консоль.
        /// </summary>
        [Description("Консоль")]
        Console
    }
}