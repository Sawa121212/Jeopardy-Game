﻿using System.Threading.Tasks;
using Common.Core.Components;
using Telegram.Bot;

namespace TelegramAPI.Infrastructure.Interfaces.Managers
{
    public interface ITelegramBotManager
    {
        TelegramBotClient TelegramBotClient { get; }

        bool IsConnected { get; }

        Task<Result<bool>> StartTelegramBot();

        Task<Result<bool>> StartTelegramBot(string token);
    }
}