using Common.Core.Components;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramAPI.Test.Services.Settings
{
    public class MainTelegramMenuService : IMainTelegramMenuService
    {
        public Result<ReplyKeyboardMarkup> CreateMenu(Update update)
        {
            return Result<ReplyKeyboardMarkup>.Done(new ReplyKeyboardMarkup(new KeyboardButton($"Войти в комнату")) { OneTimeKeyboard = true, });
        }
    }
}
