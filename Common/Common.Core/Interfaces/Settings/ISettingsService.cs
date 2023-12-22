using System.Threading.Tasks;

namespace Common.Core.Interfaces.Settings
{
    public interface ISettingsService
    {
        /// <summary>
        /// Применить сохраненные настройки из файла
        /// </summary>
        void ApplySavedSettings();

        /// <summary>
        /// Сохранить настройки в файла
        /// </summary>
        Task SaveSettings();
    }
}