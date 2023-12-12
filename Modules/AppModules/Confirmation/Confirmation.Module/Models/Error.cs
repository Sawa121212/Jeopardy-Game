using Confirmation.Module.Enums;

namespace Confirmation.Module.Models
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