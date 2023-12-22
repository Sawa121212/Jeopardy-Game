namespace Infrastructure.Interfaces.Services
{
    public interface IPathService
    {
        /// <summary>
        /// Папка настроек.
        /// </summary>
        /// <returns></returns>
        string SettingsFolder { get; }

        /// <summary>
        /// Папка загрузок.
        /// </summary>
        /// <returns></returns>
        string DownloadFolder { get; }

        /// <summary>
        /// Папка с картинками, иконками и т.д.
        /// </summary>
        /// <returns></returns>
        string ImagesFolder { get; }

        /// <summary>
        /// Папка с документацией.
        /// </summary>
        string DocumentsFolder { get; }

        /// <summary>
        /// Папка с шаблонами файлов.
        /// </summary>
        string DataFolder { get; }

        string LogFolder { get; }
    }
}