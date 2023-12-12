﻿using Material.Styles.Themes.Base;

namespace Infrastructure.Interfaces.Services.Settings
{
    /// <summary>
    /// Управление настройками приложения
    /// </summary>
    public interface ISettingsService
    {
        /// <summary>
        /// Применить тему 
        /// </summary>
        /// <param name="themeMode">Тема</param>
        public abstract void SetTheme(BaseThemeMode themeMode);

        /// <summary>
        /// Применить локализацию
        /// </summary>
        /// <param name="culture">Язык</param>
        public abstract void SetCulture(string culture);

        /// <summary>
        /// Получить текущую тему 
        /// </summary>
        public abstract BaseThemeMode GetTheme();

        /// <summary>
        /// Получить текущую локализацию 
        /// </summary>
        public abstract string GetCulture();

        /// <summary>
        /// Применить сохраненные настройки из файла
        /// </summary>
        public abstract void ApplySavedSettings();
    }
}