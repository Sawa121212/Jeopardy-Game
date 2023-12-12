using System.Collections.Generic;
using Common.Extensions.Collections;
using Infrastructure.Interfaces.Logging;
using Infrastructure.Interfaces.Services;

namespace Infrastructure.Module.Services.Logging
{
    /// <inheritdoc />
    public class LogService : ILogService
    {
        private List<ILogItem> _logCache;
        private readonly ILogViewService _logViewService;
        private readonly ILogger _logger;

        public LogService(ILogViewService logViewService, ILogger logger, IPathService pathService)
    {
        _logCache = null;
        _logViewService = logViewService;
        _logger = logger;
        logger.Init(pathService.LogFolder);
    }

        /// <inheritdoc />
        public void AddLog(ILogItem logItem)
    {
        if (_logCache != null)
        {
            _logCache.Add(logItem);
            return;
        }
            
        if (logItem != null)
        {
            // ToDo _logger.Log(logItem);
            _logViewService.AddLog(logItem);
        }
    }

        /// <inheritdoc />
        public void AddLogs(IEnumerable<ILogItem> logItems)
    {
        List<ILogItem> logs = logItems.AsList();
        foreach (ILogItem logItem in logs)
        {
            // ToDo _logger.Log(logItem);
        }
        _logViewService.AddLogs(logs);
    }

        /// <inheritdoc />
        public void BeginCache()
    {
        _logCache = new List<ILogItem>();
    }

        /// <inheritdoc />
        public void Flush()
    {
        try
        {
            if (_logCache != null)
                AddLogs(_logCache);
        }
        finally
        {
            _logCache = null;
        }
    }
    }
}