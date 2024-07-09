using Confirmation.Domain.Enums;

namespace Confirmation.Domain.Models
{
    /// <summary>
    /// Ошибка.
    /// </summary>
    public class Error : DialogInfo
    {
        /// <inheritdoc />
        public Error(string title, string message, ConfirmationResultEnum buttons) : base(title, message, buttons)
        {
        }
    }
}