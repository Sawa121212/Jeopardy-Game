using System;
using System.ComponentModel;

namespace Infrastructure.Domain.Logging.Enums
{
    /// <summary>
    /// Категории элемента лога.
    /// </summary>
    [Flags, Serializable]
    public enum LogItemCategoryEnum
    {
        /// <summary>
        /// Запрет на все.
        /// </summary>
        [Description("Запрет всех категорий")]
        None = 0x0000,

        /// <summary>
        /// Событие отладки.
        /// </summary>
        [Description("Отладка")]
        Debug = 0x0001,

        /// <summary>
        /// Исключение.
        /// </summary>
        [Description("Исключение")]
        Exception = 0x0002,

        /// <summary>
        /// Сообщение.
        /// </summary>
        [Description("Информация")]
        Info = 0x0004,

        /// <summary>
        /// Предупреждение.
        /// </summary>
        [Description("Предупреждение")]
        Warning = 0x0008,

        /// <summary>
        /// Ошибка.
        /// </summary>
        [Description("Ошибка")]
        Error = 0x0010,

        /// <summary>
        /// Логировать все.
        /// </summary>
        [Description("Все категории")]
        All = 0xFFFF
    }
}
