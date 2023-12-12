using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Runtime.CompilerServices;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Interfaces.Logging;

namespace Infrastructure.Module.Services.Logging
{
    /// <summary>
    /// Логируемая сущность.
    /// </summary>
    public class LogItem : ILogItem
    {
        /// <summary>
        /// Хеш.
        /// </summary>
        private readonly int _hash;

        /// <summary>
        /// Файл класса, вызвавший конструктор.
        /// </summary>
        private readonly string _sourceClassName;

        /// <summary>
        /// Название метода, вызвавший конструктор.
        /// </summary>
        private readonly string _sourceMethodName;

        /// <inheritdoc />
        public int Number { get; private set; }

        /// <inheritdoc />
        public DateTime TimeStamp { get; }

        /// <inheritdoc />
        public string CompName { get; }

        /// <inheritdoc />
        public string UserName { get; }

        /// <inheritdoc />
        public string AppName { get; }

        /// <inheritdoc />
        public string SourceMethod =>
            $"{(string.IsNullOrEmpty(_sourceClassName) ? string.Empty : _sourceClassName)}" +
            $"{(string.IsNullOrEmpty(_sourceMethodName) ? string.Empty : '.' + _sourceMethodName)}";

        /// <inheritdoc />
        public LogItemCategoryEnum Category { get; private set; }

        /// <inheritdoc />
        public string Message { get; }

        /// <inheritdoc />
        public bool IsRepeatable { get; }


        /// <summary>
        /// Конструктор, ограничивающий создание экземпляра без параметров.
        /// </summary>
        [SuppressMessage("ReSharper", "UnusedMember.Local")]
        private LogItem()
        {
        }

        public LogItem(string message, LogItemCategoryEnum categories, [CallerMemberName] string sourceMethod = null, string username = null,
            bool repeatable = false) : this(DateTime.Now, message, sourceMethod, username, repeatable)
        {
            Category = categories;
        }


        public LogItem(DateTime time, string message, [CallerMemberName] string sourceMethod = null, string username = null, bool repeatable = false)
        {
            TimeStamp = time;
            Message = message;
            _sourceClassName = sourceMethod;
            UserName = username;
            IsRepeatable = repeatable;
            Category = LogItemCategoryEnum.Info;
            unchecked
            {
                _hash = TimeStamp.GetHashCode();
                _hash = (_hash * 397) ^ (Message?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (SourceMethod?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (UserName?.GetHashCode() ?? 0);
            }
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="number">Порядковый номер элемента лога.</param>
        /// <param name="dt">Дата и время создания элемента лога.</param>
        /// <param name="message">Сообщение.</param>
        /// <param name="appName">Наименование приложения, пишущего в лог.</param>
        /// <param name="compName">Имя компьютера.</param>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="filePathOfClass">Файл класса, вызвавший конструктор (как правило не указывается).</param>
        /// <param name="sourceMethodName">Метод-источник, вызвавший конструктор (как правило не указывается).</param>
        /// <param name="isRepeatable">Признак разрешения повторения одинаковых, подряд идущих, элементов лога.</param>
        public LogItem(int number, DateTime dt, string message,
            string compName = null, string username = null, string appName = null,
            [CallerFilePath] string filePathOfClass = null, [CallerMemberName] string sourceMethodName = null,
            bool isRepeatable = true)
        {
            Number = number;
            TimeStamp = dt;
            Message = message;
            CompName = compName ?? Environment.MachineName;
            UserName = username ?? Environment.UserName;
            AppName = appName;
            _sourceClassName = !string.IsNullOrEmpty(filePathOfClass) && filePathOfClass.IndexOfAny(Path.GetInvalidPathChars()) == -1
                ? Path.GetFileNameWithoutExtension(filePathOfClass)
                : string.Empty;
            _sourceMethodName = sourceMethodName;
            Category = LogItemCategoryEnum.Info;
            IsRepeatable = isRepeatable;

            unchecked
            {
                _hash = Number.GetHashCode();
                _hash = (_hash * 397) ^ (TimeStamp.GetHashCode());
                _hash = (_hash * 397) ^ (Message?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (CompName?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (UserName?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (AppName?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (SourceMethod?.GetHashCode() ?? 0);
                _hash = (_hash * 397) ^ (Category.GetHashCode());
                _hash = (_hash * 397) ^ (IsRepeatable.GetHashCode());
            }
        }

        /// <inheritdoc>
        /// <cref>LogItem(int, DateTime, string, string, string, string, bool)</cref>
        /// </inheritdoc>
        /// <param name="сategory">Категория элемента лога.</param>
        [SuppressMessage("ReSharper", "InvalidXmlDocComment")]
        [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
        public LogItem(int number, DateTime dt, string message, LogItemCategoryEnum сategory,
            string compName = null, string username = null, string appName = null,
            [CallerFilePath] string filePathOfClass = null, [CallerMemberName] string sourceMethod = null,
            bool isRepeatable = true)
            : this(number, dt, message, compName, username, appName, filePathOfClass, sourceMethod, isRepeatable)
        {
            Category = сategory;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="number">Порядковый номер элемента лога.</param>
        /// <param name="dt">Дата и время создания элемента лога.</param>
        /// <param name="ex">Исключение, данные которого записываются в LogItem.</param>
        /// <param name="isWriteInnerException">Признак включения данных из внутреннего исключения объекта ex.</param>
        /// <param name="appName">Наименование приложения, пишущего в лог.</param>
        /// <param name="compName">Имя компьютера.</param>
        /// <param name="username">Имя пользователя.</param>
        /// <param name="filePathOfClass">Файл класса, вызвавший конструктор (как правило не указывается).</param>
        /// <param name="sourceMethod">Метод-источник, вызвавший конструктор (как правило не указывается).</param>
        /// <param name="isRepeatable">Признак разрешения повторения одинаковых, подряд идущих, элементов лога.</param>
        [SuppressMessage("ReSharper", "MemberCanBeProtected.Global")]
        public LogItem(int number, DateTime dt, Exception ex, bool isWriteInnerException = false,
            string compName = null, string username = null, string appName = null,
            [CallerFilePath] string filePathOfClass = null, [CallerMemberName] string sourceMethod = null,
            bool isRepeatable = true)
            : this(number, dt,
                isWriteInnerException
                    ? $"{ex.Message}; InnerException: {ex.InnerException?.Message}"
                    : ex.Message,
                compName, username, appName, filePathOfClass, sourceMethod, isRepeatable)
        {
            Category = LogItemCategoryEnum.Exception;
        }


        /// <inheritdoc />
        public void SetCategory(LogItemCategoryEnum category)
        {
            Category = category;
        }

        /// <inheritdoc />
        public void SetNumber(int number)
        {
            Number = number;
        }

        /// <inheritdoc />
        public bool Equals(ILogItem other)
        {
            return Equals((object)other);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(obj, null))
                return false;
            if (ReferenceEquals(obj, this))
                return true;
            if (GetType() != obj.GetType())
                return false;
            if (obj is not ILogItem other)
                return false;

            return Number == other.Number &&
                   DateTime.Equals(TimeStamp, other.TimeStamp) &&
                   string.Equals(Message, other.Message) &&
                   string.Equals(CompName, other.CompName) &&
                   string.Equals(UserName, other.UserName) &&
                   string.Equals(AppName, other.AppName) &&
                   string.Equals(SourceMethod, other.SourceMethod) &&
                   Category == other.Category &&
                   IsRepeatable == other.IsRepeatable;
        }

        /// <inheritdoc />
        public override int GetHashCode() => _hash;

        /// <inheritdoc />
        public object Clone()
        {
            return new LogItem(Number, TimeStamp, Message,
                CompName, UserName, AppName, _sourceClassName, _sourceMethodName, IsRepeatable);
        }
    }
}