using Common.Core.Components;
using System;
using Telegram.Bot.Types;
using Users.Domain;

namespace Users.Infrastructure
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