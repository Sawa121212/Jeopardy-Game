using System.IO;
using System.Threading.Tasks;
using Common.Core.Threading;
using Infrastructure.Domain.Settings;
using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services;
using Infrastructure.Interfaces.Services.Settings;

namespace Infrastructure.Module.Services.Settings
{
    public class ApplicationSettingsRepositoryService : IApplicationSettingsRepositoryService
    {
        private readonly ISerializableSettingsManager _settingsManager;
        private readonly string _settingsFilename;
        private readonly SmartLocker _fileLocker;

        public ApplicationSettingsRepositoryService(ISerializableSettingsManager settingsManager, IPathService pathService)
        {
            _settingsManager = settingsManager;
            _fileLocker = new SmartLocker();
            _settingsFilename = Path.Combine(pathService.SettingsFolder, "Settings.config");
        }

        public async Task<ApplicationSettings> Get()
        {
            if (!File.Exists(_settingsFilename))
                await CreateDefault();

            ApplicationSettings settings = await Task.Run(() =>
            {
                if (_fileLocker.Enter(0))
                {
                    try
                    {
                        return _settingsManager?.GetSettings<ApplicationSettings>(_settingsFilename, true);
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

        public async Task Save(ApplicationSettings settings)
        {
            await Task.Run(() =>
            {
                if (_fileLocker.Enter(0))
                {
                    try
                    {
                        _settingsManager?.SetSettings(settings, _settingsFilename, true);
                    }
                    finally
                    {
                        _fileLocker.Leave();
                    }
                }
            });
        }

        private async Task CreateDefault()
        {
            await Save(new ApplicationSettings());
        }
    }
}