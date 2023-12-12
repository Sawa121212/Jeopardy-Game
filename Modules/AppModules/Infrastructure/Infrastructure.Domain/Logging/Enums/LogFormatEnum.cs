using System;
using System.ComponentModel;

namespace Infrastructure.Domain.Logging.Enums
{
    /// <summary>
    /// Форматы лога.
    /// </summary>
    [Serializable]
    public enum LogFormatEnum
    {
        /// <summary>
        /// Простой (дата, время, сообщение).
        /// </summary>
        [Description("Простой")]
        Simple = 1,
            
        /// <summary>
        /// Полный (дата, время, компьютер, пользователь, категория, сообщение).
        /// </summary>
        [Description("Полный")]
        Full
    }
}