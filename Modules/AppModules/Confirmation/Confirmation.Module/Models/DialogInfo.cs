using Confirmation.Module.Enums;

namespace Confirmation.Module.Models
{
    /// <summary>
    /// Информация о диалоге.
    /// </summary>
    public abstract class DialogInfo
    {
        protected DialogInfo(string title, string message, ConfirmationResultEnum buttons)
        {
            Title = title;
            Message = message;
            Buttons = buttons;
        }
        
        /// <summary>
        /// Заголовок окна.
        /// </summary>
        public string Title { get; }
        
        /// <summary>
        /// Сообщение пользователю.
        /// </summary>
        public string Message { get; }
        
        /// <summary>
        /// Информация о кнопках подтверждения.
        /// </summary>
        public ConfirmationResultEnum Buttons { get; }  
    }
}