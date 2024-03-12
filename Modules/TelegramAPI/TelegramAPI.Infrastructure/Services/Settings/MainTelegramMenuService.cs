using Common.Core.Components;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;

namespace TelegramAPI.Infrastructure.Services.Settings
{
    public class MainTelegramMenuService : IMainTelegramMenuService
    {
        public Result<ReplyKeyboardMarkup> CreateMenu(Update update)
        {
            return Result<ReplyKeyboardMarkup>.Done(new ReplyKeyboardMarkup(new KeyboardButton($"Войти в комнату")) { OneTimeKeyboard = true, });
        }
    }
}
