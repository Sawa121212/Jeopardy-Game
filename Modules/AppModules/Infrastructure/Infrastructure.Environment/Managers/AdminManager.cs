using System;
using System.Threading.Tasks;
using Common.Core.Components;
using Common.Extensions;
using Infrastructure.Domain.Helpers;
using Infrastructure.Interfaces.Managers;
using TelegramAPI.Infrastructure.Interfaces.Managers;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;
using Telegram.Bot.Types;
using Users.Domain.Models;

namespace Infrastructure.Environment.Managers
{
    public class AdminManager : IAdminManager
    {
        public AdminManager(
            ITelegramSettingsService telegramSettingsService,
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

        private Result<Tuple<StateUserEnum, string>> CheckAddedAdminMode(long chatId, string key)
        {
            if (key.IsNullOrEmpty())
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет текста");
            }

            if (key == _adminModeKey)
            {
                _telegramSettingsService.SetAdminUserIdToken(chatId.ToString());
                CancelAddAdminMode();
                _telegramBotService.SendMessageAsync(chatId, $"Вы стали администратором");

                return Result<Tuple<StateUserEnum, string>>.Done(new Tuple<StateUserEnum, string>(StateUserEnum.MainMenu,
                    "Вы перешли в меню"));
            }

            return Result<Tuple<StateUserEnum, string>>.Fail("Не правильный код");
        }

        public Result<Tuple<StateUserEnum, string>> CheckAddedAdminMode(Update update)
        {
            Message message = update?.Message;

            if (message == null)
            {
                return Result<Tuple<StateUserEnum, string>>.Fail("Нет сообщения");
            }

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                Telegram.Bot.Types.User user = message.From;

                if (user == null)
                {
                    return Result<Tuple<StateUserEnum, string>>.Fail("Нет юзера");
                }

                return CheckAddedAdminMode(user.Id, message.Text);
            }

            return Result<Tuple<StateUserEnum, string>>.Fail("тип не текстовый...");
        }

        private readonly ITelegramBotService _telegramBotService;
        private readonly ITelegramSettingsService _telegramSettingsService;

        private string _adminModeKey;
    }
}