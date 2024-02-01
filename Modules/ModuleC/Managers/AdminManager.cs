using Common.Core.Components;
using Common.Extensions;
using Infrastructure.Domain.Helpers;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using TelegramAPI.Test.Services.Settings;
using Users.Domain.Models;

namespace TelegramAPI.Test.Managers
{
    public class AdminManager : IAdminManager
    {
        public AdminManager(ITelegramSettingsService telegramSettingsService,
                            ITelegramBotService telegramBotService)
        {
            _telegramBotService = telegramBotService;
            _telegramSettingsService = telegramSettingsService;
        }

        /// <inheritdoc />
        public async Task<string> VerifyAddAdminMode(long chatId)
        {
            await _telegramSettingsService.SetAdminUserIdToken(null).ConfigureAwait(true);

            _adminModeKey = RandomGenerator.GenerateFormattedSixDigitRandomNumber();
            await _telegramBotService.SendMessageAsync(chatId, $"Вторая часть кода подтверждения: ***{_adminModeKey.Substring(3, 3)}" +
                                           $"\nОтправьте код, соединив обе части в течение 2 минут\n");
            return _adminModeKey.Substring(0, 3);
        }

        /// <inheritdoc />
        public void CancelAddAdminMode()
        {
            _adminModeKey = null;
        }

        private Result<StateUserEnum> CheckAddedAdminMode(long chatId, string key)
        {
            if (key.IsNullOrEmpty())
            {
                return Result<StateUserEnum>.Fail("Нет текста");
            }

            if (key == _adminModeKey)
            {
                _telegramSettingsService.SetAdminUserIdToken(chatId.ToString());
                CancelAddAdminMode();
                _telegramBotService.SendMessageAsync(chatId, $"Вы стали администратором");
                return Result<StateUserEnum>.Done(StateUserEnum.MainMenu);
            }
            return Result<StateUserEnum>.Fail("Не правильный код");
        }

        public Result<StateUserEnum> CheckAddedAdminMode(Message message)
        {
            if (message == null)
            {
                return Result<StateUserEnum>.Fail("Нет сообщения");
            }

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Telegram.Bot.Types.User user = message.From;

                if (user == null)
                {
                    return Result<StateUserEnum>.Fail("Нет юзера");
                }

                return CheckAddedAdminMode(user.Id, message.Text);
            }
            return Result<StateUserEnum>.Fail("тип не текстовый...");

        }

        private readonly ITelegramBotService _telegramBotService;
        private readonly ITelegramSettingsService _telegramSettingsService;

        private string _adminModeKey;
    }
}