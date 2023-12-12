using Confirmation.Module.Enums;

namespace Confirmation.Module.Models
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