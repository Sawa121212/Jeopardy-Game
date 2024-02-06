using System;
using Common.Core.Components;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
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
        /// <param name="replyKeyboardMarkup"></param>
        /// <returns></returns>
        ITelegramHandlerService RegisterHandler(StateUserEnum stateUser,
            Func<Update, Result<Tuple<StateUserEnum, string>>> handler,
            Func<Update, Result<ReplyKeyboardMarkup>> replyKeyboardMarkupGenerator);

        /// <summary>
        /// Обработка сообщения.
        /// </summary>
        /// <param name="message"></param>
        Result<Tuple<StateUserEnum, string>> Handle(Update message);

        /// <summary>
        /// Получить юзера из апдейта
        /// </summary>
        /// <param name="update"></param>
        /// <returns></returns>
        Result<Telegram.Bot.Types.User> GetUser(Update update);

        /// <summary>
        /// При смене статуса - вызывай этот метод
        /// </summary>
        /// <param name="stateUser"></param>
        /// <param name="update"></param>
        /// <returns></returns>
        Result<ReplyKeyboardMarkup> GetKeyboardMarkup(StateUserEnum stateUser, Update update);
    }
}