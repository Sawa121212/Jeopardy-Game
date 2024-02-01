using System;
using Common.Core.Components;
using Telegram.Bot.Types;
using Users.Domain.Models;

namespace Users.Infrastructure.Interfaces
{
    /// <summary>
    /// Обработчик сообщений полученных ботом.
    /// </summary>
    public interface ITelegramHandlerService
    {
        /// <summary>
        /// Регистрация обработчиков.
        /// </summary>
        /// <param name="stateUser"></param>
        /// <param name="handler"></param>
        /// <returns></returns>
        ITelegramHandlerService RegisterHandler(StateUserEnum stateUser, Func<Message, Result<StateUserEnum>> handler);

        /// <summary>
        /// Обработка сообщения.
        /// </summary>
        /// <param name="message"></param>
        Result<StateUserEnum> Handle(Message message);
    }
}