using System;
using System.Threading.Tasks;
using Avalonia.Threading;
using Common.Core.Interfaces.Settings;
using Common.Core.Localization;
using TelegramAPI.Domain.Settings;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;

namespace TelegramAPI.Infrastructure.Services.Settings
{
    public class TelegramSettingsService : ITelegramSettingsService
    {
        private readonly ITelegramSettingsRepositoryService _telegramSettingsRepositoryService;
        private readonly TelegramSettings _currentTelegramSettings;

        public TelegramSettingsService(
            ITelegramSettingsRepositoryService telegramSettingsRepositoryService,
            IResourceService resourceService)
        {
            _telegramSettingsRepositoryService = telegramSettingsRepositoryService;
            _currentTelegramSettings = new TelegramSettings();
        }

        /// <inheritdoc/>
        public async void ApplySavedSettings()
        {
            try
            {
                ISettings? settings = await _telegramSettingsRepositoryService.Get();
                if (settings is not TelegramSettings telegramSettings)
                {
                    return;
                }

                // get
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _currentTelegramSettings.GameBotToken = telegramSettings.GameBotToken;
                    _currentTelegramSettings.AdminUserId = telegramSettings.AdminUserId;
                }, DispatcherPriority.SystemIdle);


                // lang
                /*await Dispatcher.UIThread.InvokeAsync(
                    () => { OnChangeCulture(); },
                    DispatcherPriority.SystemIdle);*/
            }
            catch (Exception e)
            {
                //_exceptionService.ShowException(e);
            }
        }


        /// <summary>
        /// Обновить настройки в файле
        /// </summary>
        public async Task SaveSettings()
        {
            try
            {
                TelegramSettings telegramSettings = new();
                ISettings? settings = await _telegramSettingsRepositoryService.Get();

                if (settings is TelegramSettings setting)
                {
                    telegramSettings = setting;
                }

                telegramSettings.GameBotToken = _currentTelegramSettings.GameBotToken ?? string.Empty;
                telegramSettings.AdminUserId = _currentTelegramSettings.AdminUserId ?? string.Empty;

                await _telegramSettingsRepositoryService.Save(telegramSettings);
            }
            catch (Exception e)
            {
                //_exceptionService.ShowException(e);
            }
        }

        /// <inheritdoc/>
        public string GetGameBotToken() => _currentTelegramSettings.GameBotToken;

        /// <inheritdoc/>
        public long GetAdminUserId()
        {
            return long.TryParse(_currentTelegramSettings.AdminUserId, out long value) ? value : -1;
        }

        /// <inheritdoc/>
        public async Task SetGameBotToken(string token)
        {
            _currentTelegramSettings.GameBotToken = token;
            await SaveSettings();
        }

        /// <inheritdoc/>
        public async Task SetAdminUserIdToken(string id)
        {
            _currentTelegramSettings.AdminUserId = id;
            await SaveSettings();
        }
    }
}