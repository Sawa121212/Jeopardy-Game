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
        public User CreateUser(long userId, string name)
        {
            User user = new()
            {
                Id = userId,
                Name = name,
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

        /// <inheritdoc />
        public User GetUserById(long userId)
        {
            return _dbContext.Users.Find(userId);
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
            // обновление других свойств
            _dbContext.SaveChanges();
        }

        private readonly UserDbContext _dbContext;
    }
}