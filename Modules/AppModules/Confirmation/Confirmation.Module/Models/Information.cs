using Confirmation.Module.Enums;

namespace Confirmation.Module.Models
{
    /// <summary>
    /// Информационный диалог.
    /// </summary>
    public class Information : DialogInfo
    {
        /// <inheritdoc />
        public Information(string title, string message, ConfirmationResultEnum buttons) : base(title, message, buttons)
        {
        }
    }
}