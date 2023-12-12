using System.Windows.Input;
using Common.Core.Localization;
using Common.Extensions;
using Infrastructure.Interfaces.Services.Settings;
using Material.Styles.Themes.Base;
using Prism.Commands;
using ReactiveUI;

namespace JeopardyGame.Views.Settings
{
    /// <summary>
    /// Основные настройки приложения
    /// </summary>
    public class BaseSettingsViewModel : ReactiveObject
    {
        private BaseThemeMode _themeMode;
        private BaseThemeMode _currentThemeMode;
        private LanguagesEnum _appCultureInfo;
        private LanguagesEnum _currentAppCultureInfo;
        private readonly ISettingsService _settingsService;

        public BaseSettingsViewModel(ISettingsService settingsService)
        {
            _settingsService = settingsService;
            ChangeLanguageCommand = new DelegateCommand(OnChangeLanguage);
            ChangeMaterialUiThemeCommand = new DelegateCommand(OnChangeMaterialUiTheme);
            Initialize();
        }

        private void Initialize()
        {
            CultureInfo = _settingsService.GetCulture().ToEnum<LanguagesEnum>();
            ThemeMode = _settingsService.GetTheme();
            _currentAppCultureInfo = _appCultureInfo;
            _currentThemeMode = _themeMode;
        }

        /// <summary>
        /// Поменять язык
        /// </summary>
        private void OnChangeLanguage()
        {
            if (!_currentAppCultureInfo.Equals(_appCultureInfo))
            {
                _settingsService.SetCulture(CultureInfo.ToString());
                _currentAppCultureInfo = _appCultureInfo;
            }
        }

        /// <summary>
        /// Поменять тему
        /// </summary>
        private void OnChangeMaterialUiTheme()
        {
            if (!_currentThemeMode.Equals(_themeMode))
            {
                _settingsService.SetTheme(ThemeMode);
                _currentThemeMode = _themeMode;
            }
        }

        /// <summary>
        /// Язык приложения
        /// </summary>
        public LanguagesEnum CultureInfo
        {
            get => _appCultureInfo;
            set => this.RaiseAndSetIfChanged(ref _appCultureInfo, value);
        }

        /// <summary>
        /// Тема приложения
        /// </summary>
        public BaseThemeMode ThemeMode
        {
            get => _themeMode;
            set => this.RaiseAndSetIfChanged(ref _themeMode, value);
        }

        public ICommand ChangeLanguageCommand { get; }
        public ICommand ChangeMaterialUiThemeCommand { get; }
    }
}