using System.Threading.Tasks;

namespace Common.Core.Interfaces.Settings
{
    public interface ISettingsRepositoryService
    {
        /// <summary>
        /// Получить настройки журнала регистрации изменений.
        /// </summary>
        /// <returns></returns>
        Task<ISettings?> Get();

        /// <summary>
        /// Сохранить настройки журнала регистрации изменений.
        /// </summary>
        /// <param name="settings"></param>
        Task Save(ISettings settings);
    }
}