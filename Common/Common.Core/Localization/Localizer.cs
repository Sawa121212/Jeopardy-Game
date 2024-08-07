﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Resources;
using Common.Core.Properties;

namespace Common.Core.Localization
{
    /// <summary>
    /// Локализатор
    /// </summary>
    public class Localizer : ILocalizer, INotifyPropertyChanged
    {
        private readonly string _defaultLanguage = LanguagesEnum.ru.ToString();

        private const string IndexerName = "Item";
        private const string IndexerArrayName = "Item[]";
        private List<ResourceManager>? _resources;
        private static List<ResourceManager>? _resourceManagers = new();
        public event PropertyChangedEventHandler? PropertyChanged;

        public Localizer()
        {
            LoadLanguage();

            // добавим ресурсы с текущего (Core) модуля
            AddResourceManager(new ResourceManager(typeof(Language)));
        }

        public void LoadLanguage()
        {
            if (_resourceManagers != null)
            {
                _resources = new List<ResourceManager>(_resourceManagers);
            }

            InvalidateEvents();
        }

        public void EditLanguage(string language)
        {
            Instance.ChangeLanguage(language);
        }

        /// <summary>
        /// Add Resource to Collection
        /// Добавить Ресурс в коллекцию
        /// </summary>
        /// <param name="resourceManager"></param>
        public void AddResourceManager(ResourceManager resourceManager)
        {
            _resourceManagers?.Add(resourceManager);
        }

        /// <summary>
        /// Change localization
        /// Изменить локализацию
        /// </summary>
        /// <param name="language"></param>
        public void ChangeLanguage(string language)
        {
            if (string.IsNullOrEmpty(language))
            {
                language = _defaultLanguage;
            }

            CultureInfo.CurrentUICulture = new CultureInfo(language);
            LoadLanguage();
        }

        /// <summary>
        /// Get Expression
        /// Получить Выражение
        /// </summary>
        /// <param name="key">Ключ</param>
        public string this[string key]
        {
            get
            {
                if (_resources == null || !_resources.Any())
                    return "<ERROR! LANGUAGE Resources is empty>";

                string? row = GetExpression(key);

                if (string.IsNullOrEmpty(row))
                    return $"<ERROR! Not found key \"{key}\">";

                string? ret = row?.Replace(@"\\n", "\n");
                if (string.IsNullOrEmpty(ret))
                {
                    ret = $"Localize:{key}";
                }

                return ret;
            }
        }

        /// <summary>
        /// Get Expression from Resources by Key
        /// Получить Выражение из Ресурсов по ключу
        /// </summary>
        /// <param name="key">Ключ</param>
        /// <returns></returns>
        public string GetExpression(string key)
        {
            if (_resources == null) return string.Empty;
            foreach (ResourceManager? resource in _resources)
            {
                string? row = resource?.GetString(key);
                if (!string.IsNullOrEmpty(row))
                    return row;
            }

            return string.Empty;
        }

        private void InvalidateEvents()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerName));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(IndexerArrayName));
        }

        /// <summary>
        /// Instance / Экземпляр
        /// </summary>
        public static Localizer Instance { get; } = new();
    }
}