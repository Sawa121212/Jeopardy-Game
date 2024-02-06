using Common.Core.Components;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramAPI.Test.Services.Settings
{
    /// <summary>
    /// Создает кнопки в главном меню
    /// </summary>
    public interface IMainTelegramMenuService
    {
        Result<ReplyKeyboardMarkup> CreateMenu(Update arg);
    }
}
