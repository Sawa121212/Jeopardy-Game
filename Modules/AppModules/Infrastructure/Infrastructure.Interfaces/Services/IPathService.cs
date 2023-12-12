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
        /// Папка с шаблонами проектов.
        /// </summary>
        string ProjectTemplatesFolder { get; }

        /// <summary>
        /// Папка с проектами по умолчанию.
        /// </summary>
        string ProjectsFolder { get; }

        /// <summary>
        /// Папка с конфигурацией спрайтов.
        /// </summary>
        string SpritesFolder { get; }

        /// <summary>
        /// Папка с документацией.
        /// </summary>
        string DocumentsFolder { get; }

        /// <summary>
        /// Папка с шаблонами файлов.
        /// </summary>
        string DataFolder { get; }

        /// <summary>
        /// Папка с деревом стандарта.
        /// </summary>
        string TreeStructures { get; }

        /// <summary>
        /// Папка с XSD файлами.
        /// </summary>
        string XSDFolder { get; }

        string LogFolder { get; }
    }
}