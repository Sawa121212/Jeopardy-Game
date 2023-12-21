using System.Threading.Tasks;
using ReactiveUI;
using Telegram.Bot;

namespace TelegramAPI.Test.Managers
{
    public interface ITelegramBotManager
    {
        TelegramBotClient TelegramBotClient { get; }

        Task<string> VerifyAddAdminMode(long chatId);
        void CancelAddAdminMode();
    }
}