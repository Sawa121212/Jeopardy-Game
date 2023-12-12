using System;
using Infrastructure.Domain.Logging.Enums;

namespace Infrastructure.Interfaces.Logging
{
    /// <summary>
    /// Интерфейс сущности с информацией о сообщении для логирования (элемент лога).
    /// </summary>
    public interface ILogItem : IEquatable<ILogItem>, ICloneable
    {
        /// <summary>
        /// Порядковый номер элемента лога.
        /// </summary>
        int Number { get; }

        /// <summary>
        /// Дата и время создания элемента лога.
        /// </summary>
        public DateTime TimeStamp { get; }

        /// <summary>
        /// Имя компьютера.
        /// </summary>
        string CompName { get; }

        /// <summary>
        /// Имя текущего пользователя.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Наименование приложения, пишущего в лог.
        /// </summary>
        string AppName { get; }

        /// <summary>
        /// Полное название метода, создавшего элемент лога.
        /// </summary>
        string SourceMethod { get; }

        /// <summary>
        /// Категория элемента лога.
        /// </summary>
        LogItemCategoryEnum Category { get; }

        /// <summary>
        /// Текст сообщения.
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Признак разрешения повторения одинаковых, подряд идущих, элементов лога.
        /// </summary>
        bool IsRepeatable { get; }

        /// <summary>
        /// Устанавливаем категории, в которые входит элемент лога.
        /// </summary>
        void SetCategory(LogItemCategoryEnum category);

        /// <summary>
        /// Устанавливаем порядковый номер элемента лога.
        /// </summary>
        void SetNumber(int number);
    }
}