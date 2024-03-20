using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Common.Extensions;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Environment.Services.Logging;
using Infrastructure.Interfaces.Logging;
using Infrastructure.Interfaces.Managers;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Environment.Managers
{
    /// <inheritdoc />
    public class SerializableSettingsManager : ISerializableSettingsManager
    {
        private readonly ILogService _logService;
        private readonly IPathService _pathService;
        private readonly ISerializeService _serializeService;

        /// <summary>
        /// Словарь с именами файлов настроек.
        /// </summary>
        private readonly Dictionary<string, string> _settingsFilesCache;

        public SerializableSettingsManager( /*ILogService logService,*/ IPathService pathService,
            IProtobufSerializeService serializeService)
        {
            //_logService = logService;
            _pathService = pathService;
            _serializeService = serializeService;
            _settingsFilesCache = new Dictionary<string, string>();
        }

        /// <inheritdoc />
        public void RegisterSettings<TSettings>(string fileName, TSettings defaultInstance = default(TSettings))
            where TSettings : class
        {
            string settingsName = typeof(TSettings).Name;

            //Заносим настройки в словарь.
            if (!string.IsNullOrEmpty(fileName) && !_settingsFilesCache.ContainsKey(settingsName))
                _settingsFilesCache.Add(settingsName, fileName);

            //Создаем файл настроек, если еще не создан.
            if (defaultInstance != null && !File.Exists(fileName))
            {
                SetSettings(defaultInstance, fileName);
            }
        }

        /// <inheritdoc />
        public TSettings GetSettings<TSettings>(string filename = null, bool truePath = false) where TSettings : class
        {
            TSettings settings = default(TSettings);
            string settingsName = typeof(TSettings).Name;

            if (!string.IsNullOrEmpty(filename) || _settingsFilesCache.TryGetValue(settingsName, out filename))
            {
                ProcessException.TryCatch(() =>
                {
                    if (!truePath)
                        filename = Path.Combine(_pathService.SettingsFolder, filename);

                    settings = _serializeService.Deserialize<TSettings>(filename);
                }, exp =>
                {
                    _logService?.AddLog(new LogItem($"Отсутствует файл настроек {filename}",
                        LogItemCategoryEnum.Warning));
                    Debug.Fail($"=Warning= Отсутствует файл настроек {filename}");
                });
            }

            return settings;
        }

        /// <inheritdoc />
        public void SetSettings<TSettings>(TSettings instance, string filename = null, bool truePath = false)
            where TSettings : class
        {
            string settingsName = typeof(TSettings).Name;
            if (!string.IsNullOrEmpty(filename) || _settingsFilesCache.TryGetValue(settingsName, out filename))
            {
                if (!truePath)
                    filename = Path.Combine(_pathService.SettingsFolder, filename);

                ProcessException.TryCatch(() => _serializeService.Serialize(filename, instance),
                    exp =>
                    {
                        if (_logService != null)
                        {
                            _logService.AddLog(new LogItem(
                                $"Ошибка при сохранении файла настроек {settingsName}.", LogItemCategoryEnum.Warning));
                            _logService.AddLog(new LogItem($"{exp.Flatten()}", LogItemCategoryEnum.Debug));

                            Debug.Fail($"=Warning= Ошибка при сохранении файла настроек {settingsName}.");
                            Debug.Fail($"=Warning= {exp.Flatten()}.");
                        }
                    });
            }
        }
    }
}