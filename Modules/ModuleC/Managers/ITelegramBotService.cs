using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAPI.Test.Models;

namespace TelegramAPI.Test.Managers
{
    public interface ITelegramBotService
    {
        Task<Message?> SendMessageAsync(long chatId, string message);

        Task<Message?> ForwardMessageAsync(long chatId, long fromChatId, long messageId);

        /// <summary>
        /// Use this method to send photos. On success, the sent Message is returned.
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat</param>
        /// <param name="photo">Photo to send. You can either pass a file_id as String to resend a photo that is already on the Telegram servers, or upload a new photo using multipart/form-data.</param>
        /// <param name="caption">Optional. Photo caption (may also be used when resending photos by file_id).</param>
        /// <param name="replyToMessageId">Optional. If the message is a reply, ID of the original message</param>
        /// <returns>On success, the sent Message is returned.</returns>
        Task<Message?> SendPhotoAsync(long chatId, string photo, string caption = "", int replyToMessageId = 0);

        Task<MessageModel?> ParseMessageAsync(Message message);

        Task<string> VerifyAddAdminMode(long chatId);
        void CancelAddAdminMode();
    }
}