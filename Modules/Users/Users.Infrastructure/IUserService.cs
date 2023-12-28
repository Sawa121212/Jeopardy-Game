using System.Collections.Generic;
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
        public User CreateUser(long userId, string name);

        /// <summary>
        /// Редактирование существующего пользователя
        /// </summary>
        /// <param name="user"></param>
        public void UpdateUser(User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        public void DeleteUser(User user);

        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="userId">Id пользователя</param>
        public void DeleteUser(long userId);

        /// <summary>
        /// Получение пользователя по идентификатору
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public User GetUserById(long userId);

        /// <summary>
        /// Получение всех пользователей
        /// </summary>
        /// <returns></returns>
        public IList<User> GetAllUsers();
    }
}