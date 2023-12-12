using System;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Module.Services.Settings
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        /// <summary>
        /// Строка подключения к БД
        /// </summary>
        private string DbConnectionString { get; set; }

        /// <summary>
        /// Создаёт экземпляр конфигурации приложения
        /// </summary>
        public ApplicationSettingsService()
    {
        DbConnectionString = Environment.GetEnvironmentVariable("DbConnectionString");
    }

        public string GetDbConnectionString()
    {
        return DbConnectionString;
    }
    }
}
