namespace Infrastructure.Interfaces.Services
{
    /// <summary>
    /// Служба предоставляет данные о приложении.
    /// </summary>
    public interface IApplicationInfoService
    {
        /// <summary>
        /// Получить название приложения.
        /// </summary>
        /// <returns></returns>
        string GetApplicationName();

        /// <summary>
        /// Получить версию приложения.
        /// </summary>
        /// <returns></returns>
        string GetApplicationVersion();

        /// <summary>
        /// Получить название и версию приложения.
        /// </summary>
        /// <returns></returns>
        string GetFullApplicationName();

        /// <summary>
        /// Получить заголовок приложения.
        /// </summary>
        /// <returns></returns>
        string GetApplicationTitle();

        /// <summary>
        /// Задать название приложения.
        /// </summary>
        /// <param name="applicationName"></param>
        void SetApplicationName(string applicationName);

        /// <summary>
        /// Задать версию приложения.
        /// </summary>
        /// <param name="applicationVersion"></param>
        void SetApplicationVersion(string applicationVersion);
    }
}