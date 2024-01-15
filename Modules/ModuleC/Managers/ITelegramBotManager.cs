using Common.Core.Components;
using System.Threading.Tasks;
using Telegram.Bot;

namespace TelegramAPI.Test.Managers
{
    public interface ITelegramBotManager
    {
        TelegramBotClient TelegramBotClient { get; }

        Task<Result<bool>> StartTelegramBot(string token);
    }
}