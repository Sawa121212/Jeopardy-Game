using Common.Core.Components;
using System.Collections.Generic;
using System.Linq;
using Users.Domain;

namespace Users.Infrastructure
{
    /// <inheritdoc />
    public class UserService : IUserService
    {
        public UserService()
        {
            _dbContext = new UserDbContext();
        }

        /// <inheritdoc />
        public User CreateUser(long userId, string name, string nick)
        {
            User user = new()
            {
                Id = userId,
                Name = name,
                Nick = nick,
                State = StateUserEnum.SetName,
            };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        /// <inheritdoc />
        public void DeleteUser(User question)
        {
            if (_dbContext.Users.Contains(question))
            {
                DeleteUser(question.Id);
            }
        }

        /// <inheritdoc />
        public void DeleteUser(long questionId)
        {
            User user = _dbContext.Users.Find(questionId);
            if (user == null)
            {
                return;
            }

            _dbContext.Users.Remove(user);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc />
        public IList<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public bool TryGetUserById(long userId, out User user)
        {
            user = _dbContext.Users.Find(userId);
            return user != null;
        }

        /// <inheritdoc />
        public void UpdateUser(User user)
        {
            User oldUser = _dbContext.Users.Find(user.Id);
            if (oldUser == null)
            {
                return;
            }

            oldUser.Name = user.Name;
            oldUser.State = user.State;
            oldUser.Nick = user.Nick;
            // обновление других свойств
            _dbContext.SaveChanges();
        }

        public bool TryGetUserByNiсk(string nick, out User user)
        {
            user = _dbContext.Users.FirstOrDefault(x => x.Nick == nick);
            return user != null;
        }

        public Result<StateUserEnum> UpdateUsername(Telegram.Bot.Types.Message message)
        {
            if (message == null)
            {
                return Result<StateUserEnum>.Fail("Сообщения нет");
            }

            if (message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            {
                if (TryGetUserById(message.From.Id, out User user))
                {
                    user.Name = message.Text;
                    UpdateUser(user);
                    return Result<StateUserEnum>.Done(StateUserEnum.MainMenu);
                }

                return Result<StateUserEnum>.Fail("Нет такого пользователя");
            }

            return Result<StateUserEnum>.Fail("Сообщение не текстовое");
        }

        private readonly UserDbContext _dbContext;
    }
}