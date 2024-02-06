using System.Threading.Tasks;
using Common.Core.Components;
using Common.Core.Interfaces.Settings;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramAPI.Test.Services.Settings
{
    /// <summary>
    /// Управление настройками Telegram бота
    /// </summary>
    public interface ITelegramSettingsService : ISettingsService
    {
        string GetGameBotToken();
        long GetAdminUserId();

        Task SetGameBotToken(string token);
        Task SetAdminUserIdToken(string id);
    }
}
