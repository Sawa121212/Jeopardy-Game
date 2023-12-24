using Common.Core.Components;
using Telegram.Bot;

namespace TelegramAPI.Test.Managers
{
    public interface ITelegramBotManager
    {
        TelegramBotClient TelegramBotClient { get; }

        Task<Result<bool>> StartTelegramBot(string token);

        Task<string> VerifyAddAdminMode(long chatId);
        void CancelAddAdminMode();
    }
}