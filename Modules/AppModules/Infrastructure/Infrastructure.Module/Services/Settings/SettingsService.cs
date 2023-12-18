using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Threading;
using Common.Core.Localization;
using Infrastructure.Domain.Settings;
using Infrastructure.Interfaces.Services.Settings;
using Material.Styles.Themes;
using Material.Styles.Themes.Base;

namespace Infrastructure.Module.Services.Settings
{
    public class SettingsService : ISettingsService
    {
        private readonly MaterialTheme _materialThemeStyles;
        private readonly IApplicationSettingsRepositoryService _applicationSettingsRepositoryService;
        private readonly IResourceService _resourceService;
        private readonly ILocalizer _localizer;
        private readonly ApplicationSettings _applicationSettings;
        private readonly string _defaultCulture = "ru";
        private readonly CultureInfo _cultureInfo;

        public SettingsService(
            IApplicationSettingsRepositoryService applicationSettingsRepositoryService, IResourceService resourceService,
            ILocalizer localizer)
        {
            _applicationSettingsRepositoryService = applicationSettingsRepositoryService;
            _resourceService = resourceService;
            _localizer = localizer;
            _materialThemeStyles = Application.Current!.LocateMaterialTheme<MaterialTheme>();
            _applicationSettings = new ApplicationSettings();
            _cultureInfo = CultureInfo.CurrentUICulture;
        }

        /// <inheritdoc/>
        public async void SetTheme(BaseThemeMode themeMode)
        {
            _applicationSettings.ThemeMode = themeMode;
            await OnSaveAppSettings();
        }

        /// <inheritdoc/>
        public async void SetCulture(string culture)
        {
            _applicationSettings.UsedCulture = culture;
            OnChangeCulture();
            await OnSaveAppSettings();
        }

        /// <inheritdoc/>
        public BaseThemeMode GetTheme() => _applicationSettings.ThemeMode;

        /// <inheritdoc/>
        public string GetCulture() => _applicationSettings.UsedCulture;

        /// <inheritdoc/>
        public async void ApplySavedSettings()
        {
            try
            {
                ApplicationSettings settings = await _applicationSettingsRepositoryService.Get();
                if (settings == null)
                {
                    return;
                }

                // get
                await Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _applicationSettings.ThemeMode = settings.ThemeMode switch
                    {
                        BaseThemeMode.Inherit => BaseThemeMode.Light,
                        BaseThemeMode.Light => BaseThemeMode.Light,
                        BaseThemeMode.Dark => BaseThemeMode.Dark,
                        _ => throw new ArgumentOutOfRangeException()
                    };

                    _applicationSettings.UsedCulture =
                        settings.UsedCulture != null ? settings.UsedCulture : _defaultCulture; // ToDo 
                }, DispatcherPriority.SystemIdle);

                // Theme
                await Dispatcher.UIThread.InvokeAsync(
                    () => { _materialThemeStyles.BaseTheme = _applicationSettings!.ThemeMode; },
                    DispatcherPriority.SystemIdle);

                // lang
                await Dispatcher.UIThread.InvokeAsync(
                    () => { OnChangeCulture(); },
                    DispatcherPriority.SystemIdle);
            }
            catch (Exception e)
            {
                //_exceptionService.ShowException(e);
            }
        }

        private void OnChangeCulture()
        {
            if (string.IsNullOrEmpty(_applicationSettings.UsedCulture))
            {
                throw new ArgumentException($"{nameof(_applicationSettings.UsedCulture)} can't be empty.");
            }

            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(_applicationSettings.UsedCulture);

            _resourceService.ChangeResourcesEvent();
            _localizer.EditLanguage(GetCulture());
        }

        /// <summary>
        /// Обновить настройки в файле
        /// </summary>
        private async Task OnSaveAppSettings()
        {
            await Dispatcher.UIThread.InvokeAsync(
                () => { _materialThemeStyles.BaseTheme = _applicationSettings.ThemeMode; },
                DispatcherPriority.SystemIdle);

            try
            {
                ApplicationSettings settings = await _applicationSettingsRepositoryService.Get() ?? new ApplicationSettings();
                settings.ThemeMode = _applicationSettings.ThemeMode;
                settings.UsedCulture = _applicationSettings.UsedCulture;

                await _applicationSettingsRepositoryService.Save(settings);
            }
            catch (Exception e)
            {
                //_exceptionService.ShowException(e);
            }
        }
    }
}