using Common.Core.Components;
using System.Collections.Generic;
using Telegram.Bot.Types;
using Users.Domain;

namespace Users.Infrastructure
{
    /// <summary>
    /// Сервис для управления пользователи
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Создание нового пользователя
        /// </summary>
        /// <param name="userId"></param>
        public Domain.User CreateUser(long userId, string name, string nick);

        /// <summary>
        /// Редактирование существующего пользователя
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(Domain.User user);

        /// <summary>
        /// Редактирование имени существующего пользователя
        /// </summary>
        /// <param name="message"></param>
        public Result<StateUserEnum> UpdateUsername(Message message);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        public void DeleteUser(Domain.User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        public void DeleteUser(long userId);

        /// <summary>
        /// Попытка получения пользователя по нику
        /// </summary>
        /// <param name="nick"></param>
        /// <returns></returns>
        public bool TryGetUserByNiсk(string nick, out Domain.User user);

        /// <summary>
        /// Попытка получения пользователя по идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool TryGetUserById(long userId, out Domain.User user);

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns></returns>
        public IList<Domain.User> GetAllUsers();
    }
}