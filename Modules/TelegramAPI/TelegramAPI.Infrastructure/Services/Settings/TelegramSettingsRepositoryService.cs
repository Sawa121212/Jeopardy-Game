using System.IO;
using System.Threading.Tasks;
using Common.Core.Interfaces.Settings;
using Common.Core.Threading;
using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services;
using TelegramAPI.Domain.Settings;
using TelegramAPI.Infrastructure.Interfaces.Services.Settings;

namespace TelegramAPI.Infrastructure.Services.Settings
{
    public class TelegramSettingsRepositoryService : ITelegramSettingsRepositoryService
    {
        private readonly ISerializableSettingsManager _settingsManager;
        private readonly string _settingsFilename;
        private readonly SmartLocker _fileLocker;

        public TelegramSettingsRepositoryService(ISerializableSettingsManager settingsManager, IPathService pathService)
        {
            _settingsManager = settingsManager;
            _fileLocker = new SmartLocker();
            _settingsFilename = Path.Combine(pathService.SettingsFolder, "TelegramSettings.config");
        }

        public async Task<ISettings?> Get()
        {
            if (!File.Exists(_settingsFilename))
                await CreateDefault();

            TelegramSettings? settings = await Task.Run(() =>
            {
                if (_fileLocker.Enter(0))
                {
                    try
                    {
                        return _settingsManager.GetSettings<TelegramSettings>(_settingsFilename, true);
                    }
                    finally
                    {
                        _fileLocker.Leave();
                    }
                }

                return null;
            });

            return settings;
        }


        /// <inheritdoc />
        public async Task Save(ISettings settings)
        {
            if (settings is TelegramSettings telegramSettings)
            {
                await Task.Run(() =>
                {
                    if (_fileLocker.Enter(0))
                    {
                        try
                        {
                            _settingsManager?.SetSettings(telegramSettings, _settingsFilename, true);
                        }
                        finally
                        {
                            _fileLocker.Leave();
                        }
                    }
                });
            }
        }

        private async Task CreateDefault()
        {
            await Save(new TelegramSettings());
        }
    }
}