using System.Threading.Tasks;
using Infrastructure.Domain.Settings;

namespace Infrastructure.Interfaces.Services.Settings
{
    public interface IApplicationSettingsRepositoryService
    {
        /// <summary>
        /// Получить настройки журнала регистрации изменений.
        /// </summary>
        /// <returns></returns>
        Task<ApplicationSettings> Get();

        /// <summary>
        /// Сохранить настройки журнала регистрации изменений.
        /// </summary>
        /// <param name="settings"></param>
        Task Save(ApplicationSettings settings);
    }
}