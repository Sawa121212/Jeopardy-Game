using System;
using System.IO;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Module.Services
{
    /// <inheritdoc cref="IPathService"/>
    public class PathService : IPathService
    {
        private static readonly string LocalApplicationDataFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        ///<inheritdoc />
        public string SettingsFolder => GetLocalDirectory("Settings");

        /// <inheritdoc />
        public string DownloadFolder => GetLocalDirectory("Download");

        /// <inheritdoc />
        public string ImagesFolder => GetLocalDirectory("Images");

        /// <inheritdoc />
        public string DocumentsFolder => GetLocalDirectory("Documents");

        /// <inheritdoc />
        public string DataFolder => GetLocalDirectory("Data");

        /// <inheritdoc />
        public string LogFolder => GetLocalDirectory("Logs");

        /// <summary>
        /// Получить локальную папку приложения.
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private static string GetLocalDirectory(string directoryName)
        {
            return Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, directoryName)).FullName;
        }

        /// <summary>
        /// Получить AppDataFolder
        /// </summary>
        /// <param name="directoryName"></param>
        /// <returns></returns>
        private static string GetAppDataDirectory(string directoryName)
        {
            return Directory.CreateDirectory(Path.Combine(LocalApplicationDataFolder, directoryName)).FullName;
        }

        private static readonly string AppDataFolder =
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
    }
}