using System;
using Infrastructure.Domain.Logging.Enums;
using Infrastructure.Interfaces.Logging;

namespace Infrastructure.Environment.Services.Logging
{
    public class PositionLogItem : LogItem, IPositionReference
    {
        public PositionLogItem(string message, LogItemCategoryEnum сategory, int x, int y)
            : base(0, DateTime.Now, message, сategory)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}