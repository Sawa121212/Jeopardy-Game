using Confirmation.Domain.Enums;

namespace Confirmation.Domain.Models
{
    /// <summary>
    /// Предупреждение.
    /// </summary>
    public class Warning : DialogInfo
    {
        /// <inheritdoc />
        public Warning(string title, string message, ConfirmationResultEnum buttons) : base(title, message, buttons)
        {
        }
    }
}