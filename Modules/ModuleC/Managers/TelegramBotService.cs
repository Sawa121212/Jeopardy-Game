using System;
using System.IO;
using System.Threading.Tasks;
using Common.Extensions;
using Infrastructure.Domain.Helpers;
using ReactiveUI;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramAPI.Test.Services.Settings;

namespace TelegramAPI.Test.Managers
{
    public class TelegramBotService : ReactiveObject, ITelegramBotService
    {
        public TelegramBotService(ITelegramBotManager telegramBotManager, ITelegramSettingsService telegramSettingsService)
        {
            _telegramBotManager = telegramBotManager;
            _telegramSettingsService = telegramSettingsService;
        }

        /// <inheritdoc />
        public bool IsAddAdminMode
        {
            get => _isAddAdminMode;
            private set => this.RaiseAndSetIfChanged(ref _isAddAdminMode, value);
        }

        /// <inheritdoc />
        public async Task<Message?> SendMessageAsync(long chatId, string text)
        {
            // ToDo show Exception result
            try
            {
                Message? message = await _telegramBotManager.TelegramBotClient.SendTextMessageAsync(chatId, text);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Message?> ForwardMessageAsync(long chatId, long fromChatId, long messageId)
        {
            // ToDo show Exception result
            try
            {
                Message? message = await _telegramBotManager.TelegramBotClient.ForwardMessageAsync(chatId, fromChatId, (int) messageId);
                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }

        /// <inheritdoc />
        public async Task<Message?> SendPhotoAsync(long chatId, string photoUrl, string caption = "", int replyToMessageId = 0)
        {
            try
            {
                Message message;
                await using FileStream fileStream = new(photoUrl, FileMode.Open, FileAccess.Read, FileShare.Read);
                message = await _telegramBotManager.TelegramBotClient.SendPhotoAsync(chatId,
                    new InputFileStream(fileStream), null, caption);

                return message;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return null;
        }


        /// <inheritdoc />
        public async Task<string> VerifyAddAdminMode(long chatId)
        {
            await _telegramSettingsService.SetAdminUserIdToken(null).ConfigureAwait(true);

            _adminModeKey = RandomGenerator.GenerateFormattedSixDigitRandomNumber();
            IsAddAdminMode = true;
            await SendMessageAsync(chatId, $"Вторая часть кода подтверждения: ***{_adminModeKey.Substring(3, 3)}" +
                                      $"\nОтправьте код, соединив обе части в течение 2 минут\n");
            return _adminModeKey.Substring(0, 3);
        }

        /// <inheritdoc />
        public void CancelAddAdminMode()
        {
            _adminModeKey = null;
            IsAddAdminMode = false;
        }

        private async Task CheckAddedAdminMode(long chatId, string key)
        {
            if (!IsAddAdminMode)
            {
                return;
            }

            if (key.IsNullOrEmpty())
            {
                return;
            }

            if (key == _adminModeKey)
            {
                await _telegramSettingsService.SetAdminUserIdToken(chatId.ToString());
                CancelAddAdminMode();
                await SendMessageAsync(chatId, $"Вы стали администратором");
            }
        }

        private readonly ITelegramBotManager _telegramBotManager;
        private readonly ITelegramSettingsService _telegramSettingsService;

        private bool _isAddAdminMode;
        private string _adminModeKey;
    }
}