using System.Collections.Generic;
using Infrastructure.Interfaces.Logging;

namespace Infrastructure.Interfaces.Services
{
    public interface ILogViewService
    {
        /// <summary>
        /// Очистить отображаемые ошибки с журнале.
        /// </summary>
        void ClearErrors(int key = default(int));

        /// <summary>
        /// Добавить событие в журнал.
        /// </summary>
        /// <param name="logItem">Элемент журнала.</param>
        void AddLog(ILogItem logItem);

        /// <summary>
        /// Добавить пачку логов.
        /// </summary>
        /// <param name="logItems"></param>
        void AddLogs(IEnumerable<ILogItem> logItems);
    }
}