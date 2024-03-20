using System.Collections.Generic;
using Infrastructure.Interfaces.Logging;
using Infrastructure.Interfaces.Services;
using Prism.Events;

namespace Infrastructure.Environment.Services.Logging
{
    /// <inheritdoc />
    public class LogViewService : ILogViewService
    {
        private readonly IEventAggregator _eventAggregator;

        public LogViewService(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        /// <inheritdoc />
        public void ClearErrors(int key = default(int))
        {
            //_eventAggregator.GetEvent<ClearErrorsEvent>().Publish(new ClearErrorsEvent(key));
        }

        /// <inheritdoc />
        public void AddLog(ILogItem logItem)
        {
            //_eventAggregator.GetEvent<AddLogEvent>().Publish(logItem);
        }

        /// <inheritdoc />
        public void AddLogs(IEnumerable<ILogItem> logItems)
        {
            //_eventAggregator.GetEvent<AddLogsEvent>().Publish(logItems);
        }
    }
}