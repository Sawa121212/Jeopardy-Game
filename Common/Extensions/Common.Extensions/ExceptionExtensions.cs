﻿// Источник: http://mvvmhelpers.codeplex.com/SourceControl/latest#Julmar.Wpf.Helpers/Julmar.Core/Extensions/ExceptionExtensions.cs

using System;
using System.Text;

namespace Common.Extensions
{
    /// <summary>
    /// Методы расширения для <see cref="Exception"/>.
    /// </summary>
    public static class ExceptionExtensions
    {
        /// <summary>
        /// Получение строки с данными текущего и внутренних исключений.
        /// </summary>
        /// <param name="ex">Исключение.</param>
        /// <param name="message">Строковый префикс для добавления в результат.</param>
        /// <param name="isIncludeStackTrace">Признак добавления в результат так же и трассировку стека.</param>
        /// <returns>Строка с сообщением текущего исключения и сообщениями всех вложенных исключений,
        /// сведенными вместе.</returns>
        public static string Flatten(this Exception ex, string message = "", bool isIncludeStackTrace = false)
        {
            if (ex == null)
                return message;

            StringBuilder? sb = new();
            sb.AppendLine(message);

            do
            {
                sb.AppendLine(ex.Message);

                if (isIncludeStackTrace)
                    sb.AppendLine(ex.StackTrace);

                ex = ex.InnerException;

            } while (ex != null);

            return sb.ToString().Trim();
        }
    }
}
    