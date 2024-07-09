using System.Collections.Generic;
using System;
using System.Linq;

namespace Common.Extensions
{
    /// <summary>
    /// Оборачивает стандартные исключения в синтаксический сахар, но не только...
    /// </summary>
    public static class ProcessException
    {
        private static readonly List<Type> FatalExceptions = new()
        {
            typeof(OutOfMemoryException),
            typeof(StackOverflowException),
            //Ещё типы исключений, который по вашему мнению всегда являются фатальными
        };

        /// <summary>
        /// Отлавливает все исключения, кроме критических.
        /// </summary>
        /// <param name="tryAction">Полезное действие.</param>
        /// <param name="handlerAction">Действие по обработке исключений.</param>
        public static bool TryCatch(Action tryAction, Action<Exception> handlerAction)
        {
            try
            {
                tryAction();
            }
            catch (Exception exp)
            {
                if (IsFatal(exp)) throw;
                handlerAction(exp);
                return false;
            }
            return true;
        }

        private static bool IsFatal(Exception exp)
        {
            return FatalExceptions.Any(curExp => exp.GetType() == curExp);
        }
    }
}