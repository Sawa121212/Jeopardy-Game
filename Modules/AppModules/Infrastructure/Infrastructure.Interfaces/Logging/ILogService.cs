using System.Collections.Generic;

namespace Infrastructure.Interfaces.Logging
{
    /// <summary>
    /// Служба осуществляет логирование. Расширяет функционал ILogger.
    /// </summary>
    public interface ILogService
    {
        /// <summary>
        /// Добавить лог.
        /// </summary>
        /// <param name="logItem">Событие лога.</param>
        void AddLog(ILogItem logItem);

        /// <summary>
        /// Добавить несколько записей.
        /// </summary>
        /// <param name="logItems"></param>
        void AddLogs(IEnumerable<ILogItem> logItems);

        /// <summary>
        /// Начать кэширование.
        /// </summary>
        void BeginCache();

        /// <summary>
        /// Сохранить кэши.
        /// </summary>
        void Flush();
    }
}