using System;
using System.Diagnostics.CodeAnalysis;

namespace Common.Core.Components
{
    [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Global")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global")]
    public class Result<T>
    {
        /// <summary>
        /// Результирующее значение.
        /// </summary>
        public T Value { get; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Исключение (ошибка).
        /// </summary>
        public Exception Excptn { get; }

        /// <summary>
        /// Проверка результата на ошибки.
        /// </summary>
        public bool HasValue => (ErrorMessage == null) && (Excptn == null);

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Result(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Result(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        protected Result(Exception excptn)
        {
            Excptn = excptn;
        }


        /// <summary>
        /// При успешном выполнении.
        /// </summary>
        public static Result<T> Done(T value)
        {
            return new Result<T>(value);
        }

        /// <summary>
        /// В случае ошибки.
        /// </summary>
        public static Result<T> Fail(string errorMessage)
        {
            return new Result<T>(errorMessage);
        }

        /// <summary>
        /// В случае ошибки.
        /// </summary>
        public static Result<T> Fail(Exception excptn)
        {
            return new Result<T>(excptn);
        }

        /// <summary>
        /// Для использования if(<paramref name="result"/>).
        /// </summary>
        public static implicit operator bool(Result<T> result)
        {
            return result != null && result.HasValue;
        }
    }
}