namespace Common.Core.Components
{
    /// <summary>
    /// Для возврата значения вместо исключений.
    /// </summary>
    public record Result
    {
        /// <summary>
        /// При успешном выполнении.
        /// </summary>
        protected Result()
        {
        }

        /// <summary>
        /// В случае ошибки.
        /// </summary>
        /// <param name="errorMessage">Текст ошибки.</param>
        protected Result(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        public string ErrorMessage { get; protected set; }

        /// <summary>
        /// Проверка результата на ошибки.
        /// </summary>
        public bool HasError => ErrorMessage != null;

        /// <summary>
        /// В случае ошибки.
        /// </summary>
        /// <param name="errorMessage"></param>
        /// <returns></returns>
        public static Result Fail(string errorMessage)
        {
            return new Result(errorMessage);
        }

        /// <summary>
        /// При успешном выполнении.
        /// </summary>
        /// <returns></returns>
        public static Result Done()
        {
            return new Result();
        }

        /// <summary>
        /// Для использования if(result)
        /// </summary>
        /// <param name="result"></param>
        public static implicit operator bool(Result result)
        {
            return result != null && !result.HasError;
        }
    }
}