namespace Infrastructure.Interfaces.Managers
{
    /// <summary>
    /// Управляет сериализуемыми настройками.
    /// </summary>
    public interface ISerializableSettingsManager
    { 
        /// <summary>
        /// Добавить новый тип настроек.
        /// </summary>
        /// <typeparam name="TSettings">Конкретный тип настроек.</typeparam>
        /// <param name="filename">Имя файла настроек.</param>
        /// <param name="defaultInstance">Экземпляр настроек по умолчанию.</param>
        void RegisterSettings<TSettings>(string filename, TSettings defaultInstance = null) where TSettings : class;
        
        /// <summary>
        /// Получить настройки.
        /// </summary>
        /// <typeparam name="TSettings">Конкретный тип настроек.</typeparam>
        /// <param name="filename"></param>
        /// <param name="truePath">Использовать истинный путь.</param>
        /// <returns></returns>
        TSettings GetSettings<TSettings>(string filename = null, bool truePath = false) where TSettings : class;

        /// <summary>
        /// Сохранить настройки.
        /// </summary>
        /// <typeparam name="TSettings">Конкретный тип настроек.</typeparam>
        /// <param name="instance">Сохраняемый экземпляр настроек.</param>
        /// <param name="filename"></param>
        /// <param name="truePath">Использовать истинный путь.</param>
        void SetSettings<TSettings>(TSettings instance, string filename = null, bool truePath = false) where TSettings : class;
    }
}